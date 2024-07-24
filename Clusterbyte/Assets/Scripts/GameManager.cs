using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entity.Factory;
using Entity.Factory.Info;
using Entity.Factory.Types;
using Lib;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Viewport;

/// <summary>
/// Primary game manager that handles the game state and level progression for Clusterbyte.
/// </summary>
public class GameManager : StateManager
{
    /// <summary>
    /// Instance of the GameManager singleton.
    /// </summary>
    public static GameManager instance;
    /// <summary>
    /// Level that the player is currently on.
    /// </summary>
    public int currentLevel = 0;

    [SerializeField] private CameraPositioner cam;
    [SerializeField] private UIExtensible shopUI;
    [SerializeField] private UIExtensible viewUI;
    [SerializeField] private UIExtensible exitWarningUI;
    [SerializeField] private UIExtensible wonUI;
    [SerializeField] private UIExtensible loseUI;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private GameObject navIndicatorPrefab;

    private MouseHover mouseHover;
    private CameraPositioner mainCamera;

    /// <summary>
    /// Stats for the player, including tokens and lives.
    /// </summary>
    public PlayerStats playerStats { get; private set; }
    /// <summary>
    /// Where enemies will spawn from in this level.
    /// </summary>
    public GameObject entitySpawn { get; private set; }
    /// <summary>
    /// Where enemies will try to move to in this level.
    /// </summary>
    public GameObject entityTarget { get; private set; }

    private readonly List<GameObject> placeableTerrain = new();
    private readonly List<GameObject> deployed = new();
    private readonly List<Vector2> takenPositions = new();
    private readonly List<GameObject> enemies = new();
    private readonly List<Coroutine> waveCoroutines = new();
    private GameObject[] viewingWaveEnemies;

    /// <summary>
    /// Default state to be looking at the field and not interacting with it.
    /// </summary>
    public static GameState VIEWING;
    /// <summary>
    /// Selected an empty tile to place a defender on.
    /// </summary>
    public static GameState SHOPPING;
    /// <summary>
    /// Enemies have won.
    /// </summary>
    public static GameState DIED;
    /// <summary>
    /// Player has won.
    /// </summary>
    public static GameState WON;
    /// <summary>
    /// The game is currently active and enemies are moving.
    /// </summary>
    public static GameState ACTIVE;

    /// <summary>
    /// Parses the level mapping and spawns the terrain prefabs accordingly.
    /// </summary>
    private void ParseLevelMap(int level)
    {
        // Loop over every (x,y) grid cell in the level
        for (int y = 0; y < Clusterbyte.GRID_HEIGHT; y++)
        {
            for (int x = 0; x < Clusterbyte.GRID_WIDTH; x++)
            {
                // Query the level selector for terrain generation
                // level -> y (row) -> x (column)
                int terrainId = Clusterbyte.LEVELS_TERRAIN[level][y][x];
                // This is an empty cell, skip
                if (terrainId == 0)
                    continue;
                // Placeable terrain has id=2, inactive terrain has id=1, start has id=3, end has id=4
                Entity.Factory.Entity toSpawn = terrainId switch
                {
                    1 => EntityFactory.Get<MapElement>("Dead"),
                    2 => EntityFactory.Get<MapElement>("Placeable"),
                    3 => EntityFactory.Get<StartMarker>(),
                    4 => EntityFactory.Get<EndMarker>(),
                    _ => throw new Exception("Invalid terrainId")
                };
                // Spawn the terrain prefab at the grid cell
                // The grid is flipped in the y-axis on the Unity plane, so inversion is required
                float invertedY = Clusterbyte.GRID_HEIGHT - y;
                GameObject spawned = toSpawn.InstantiateAtGrid(x, invertedY);
                switch (terrainId)
                {
                    case 2:
                        // Must also manage the list of terrain objects that the user can place on for validation
                        placeableTerrain.Add(spawned);
                        break;
                    // Also keep track of the start and end positions
                    case 3:
                        entitySpawn = spawned;
                        break;
                    case 4:
                        entityTarget = spawned;
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Whether a position is occupied by a placed item.
    /// </summary>
    /// <param name="position">world position to check</param>
    /// <returns>whether this position is occupied by a player-placed item</returns>
    public bool IsOccupied(Vector3 position)
    {
        return takenPositions.Contains(Clusterbyte.ConvertTo2D(position));
    }

    /// <summary>
    /// Whether a position is terrain that can be placed on.
    /// </summary>
    /// <param name="position">world position to check</param>
    /// <returns>whether this position can have items placed on</returns>
    public bool IsPlaceable(Vector3 position)
    {
        return placeableTerrain.Any(t => Clusterbyte.ConvertTo2D(t.transform.position) == Clusterbyte.ConvertTo2D(position));
    }

    /// <summary>
    /// Buy and deploy an item by string lookup.
    /// </summary>
    /// <param name="itemName">the name of the item</param>
    public void BuyItem(string itemName)
    {
        // Only buy when shopping
        if (state != SHOPPING)
            return;

        // Check illegal states
        if (IsOccupied(mouseHover.hoveredPosition))
        {
            SendAlert("Position is already occupied.", Popup.Type.WARNING);
            return;
        }
        if (!EntityFactory.TryGet(itemName, out ShopDeployable item))
        {
            SendAlert($"Item {itemName} does not exist.", Popup.Type.ERROR);
            return;
        }

        // Take money and deploy item
        Vector3 position = mouseHover.hoveredPosition;
        if (playerStats.TryTransaction(item.cost))
        {
            SendAlert($"Bought {itemName} for {item.cost} tokens.");
            deployed.Add(Instantiate(item.prefab, position + new Vector3(0, 2, 0), Quaternion.identity));
            takenPositions.Add(Clusterbyte.ConvertTo2D(position));
            SetState(VIEWING);
        }
        else
        {
            SendAlert("Not enough tokens.", Popup.Type.WARNING);
        }
    }

    internal void Awake()
    {
        // Assign variables
        instance = this;
        TryGetComponent(out mouseHover);
        playerStats = GetComponent<PlayerStats>();
        playerStats.ResetStats();

        VIEWING = new GameState(ViewingInit, null, ViewingEnd);
        SHOPPING = new GameState(ShoppingInit, null, ShoppingEnd);
        DIED = new GameState(OnDeath, null, null);
        WON = new GameState(OnWin, null, null);
        ACTIVE = new GameState(ActiveInit, ActivePeriodic, ActiveEnd);

        // Returns to overview setting at default or ESC
        SetState(VIEWING);
        ChangeStateOnEvent(() => state == SHOPPING && Input.GetKeyDown(KeyCode.Escape), VIEWING);

        // On click of terrain during overview setting, switch to observing states
        ChangeStateOnEvent(() => state == VIEWING && Input.GetMouseButtonDown(0) && mouseHover.isHovering && !IsOccupied(mouseHover.hoveredPosition), SHOPPING);
    }

    internal void Start()
    {
        // TODO: states for on game done for new field stuff
        ParseLevelMap(currentLevel);
        StartCoroutine(SendViewingWave(currentLevel));
    }

    private IEnumerator SendViewingWave(int level)
    {
        yield return new WaitForSeconds(0.5f);
        // Send as many waves as there are enemies that will spawn in this level
        int enemySpawnCount = Clusterbyte.ENEMY_SPAWN_TIMES[level].Length;
        viewingWaveEnemies = new GameObject[enemySpawnCount];
        for (int i = 0; i < enemySpawnCount; i++)
        {
            viewingWaveEnemies[i] = Instantiate(navIndicatorPrefab, entitySpawn.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void SendAlert(string alertText, Popup.Type type = Popup.Type.INFO)
    {
        EntityFactory.Get<Popup>().SendText(alertText, type);
    }

    private void ViewingInit()
    {
        statusText.text = $"Level {currentLevel + 1}";
        cam.interpolateSpeed = 2;
        cam.SetPosition(CameraPositioner.CameraSpot.ANGLED);
        mouseHover.enabled = true;
        viewUI.Show();
    }

    private void ViewingEnd()
    {
        viewUI.Hide();
    }

    private void ShoppingInit()
    {
        Vector3 worldClicked = mouseHover.hoveredPosition;
        cam.interpolateSpeed = 4;
        // Move to just above the tile, looking down at it
        cam.SetCustomSpot(worldClicked + new Vector3(0, 3, -2), Quaternion.Euler(45, 0, 0));
        cam.SetPosition(CameraPositioner.CameraSpot.CUSTOM);
        shopUI.Show();
        mouseHover.enabled = false;
        statusText.text = $"Tile: ({worldClicked.x}, {worldClicked.z})";
    }

    private void ShoppingEnd()
    {
        shopUI.Hide();
    }

    private void ActiveInit()
    {
        // Set up field for viewing and disable UI
        entitySpawn.SetActive(false);
        entityTarget.SetActive(false);
        cam.interpolateSpeed = 2;
        cam.SetPosition(CameraPositioner.CameraSpot.OVERHEAD);
        mouseHover.enabled = false;

        // Parse the level map and spawn the enemies
        Tuple<float, string>[] wave = Clusterbyte.ENEMY_SPAWN_TIMES[currentLevel];
        foreach (Tuple<float, string> timePair in wave)
        {
            // If the entity does not exist, skip
            if (!EntityFactory.TryGet(timePair.Item2, out Entity.Factory.Entity e))
                continue;

            // Queue the spawning of this entity
            waveCoroutines.Add(StartCoroutine(SpawnIn(timePair.Item1, e)));
        }

        // Remove any viewing markers that have not been destroyed by the end of the wave
        foreach (GameObject viewingMarker in viewingWaveEnemies)
        {
            if (viewingMarker != null)
                Destroy(viewingMarker);
        }
    }

    private IEnumerator SpawnIn(float seconds, Entity.Factory.Entity toSpawn)
    {
        // Wait a delay and spawn the entity
        yield return new WaitForSeconds(seconds);
        Vector2 spawnPosition = Clusterbyte.ConvertTo2D(entitySpawn.transform.position);
        toSpawn.InstantiateAtGrid(spawnPosition.x, spawnPosition.y);
        enemies.Add(toSpawn.instance);
    }
    
    private bool IsSpawningDone()
    {
        // Note: The enemies array is not depopulated (in terms of length) when an enemy dies,
        // therefore this check will work as intended and not be affected by enemy deaths
        return enemies.Count == Clusterbyte.ENEMY_SPAWN_TIMES[currentLevel].Length;
    }

    private void ActivePeriodic()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            exitWarningUI.Show();

        int enemiesAlive = enemies.Count(g => g != null);
        statusText.text = IsSpawningDone() ? $"Enemies: {enemiesAlive}/{enemies.Count}" : $"Spawning: {enemies.Count}/{Clusterbyte.ENEMY_SPAWN_TIMES[currentLevel].Length}";

        // Win condition is when all enemies are dead (array is all null) and the wave is done
        if (IsSpawningDone() && enemiesAlive <= 0)
            SetState(WON);
        if (playerStats.lives <= 0)
            SetState(DIED);
    }

    private void ActiveEnd()
    {
        // Halt all waves
        foreach (Coroutine waveCoroutine in waveCoroutines)
            StopCoroutine(waveCoroutine);

        // Remove all extra enemies from the field
        enemies.ForEach(e =>
        {
            if (e != null) Destroy(e);
        });

        enemies.Clear();
        waveCoroutines.Clear();
        statusText.text = $"Level {currentLevel + 1}";
    }

    private void OnDeath()
    {
        loseUI.Show();
    }

    private void OnWin()
    {
        wonUI.Show();
    }
}