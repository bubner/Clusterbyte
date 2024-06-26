using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entity;
using Entity.Markers;
using Entity.Types;
using Lib;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Viewport;

public class GameManager : StateManager
{
    public static GameManager instance;
    public int currentLevel = 0;

    [SerializeField] private CameraPositioner cam;
    [SerializeField] private UIExtensible shopUI;
    [SerializeField] private UIExtensible viewUI;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private GameObject navIndicatorPrefab;

    private MouseHover mouseHover;
    private CameraPositioner mainCamera;

    public PlayerStats playerStats { get; private set; }
    public GameObject entitySpawn { get; private set; }
    public GameObject entityTarget { get; private set; }

    private readonly List<GameObject> placeableTerrain = new();
    private readonly List<GameObject> deployed = new();
    private readonly List<Vector2> takenPositions = new();

    public static GameState VIEWING;
    public static GameState SHOPPING;
    public static GameState EDITING;
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
                Entity.Entity toSpawn = terrainId switch
                {
                    1 => EntityFactory.Get<MapElement>("Dead"),
                    2 => EntityFactory.Get<MapElement>("Placeable"),
                    3 => EntityFactory.Get<StartMarker>(),
                    4 => EntityFactory.Get<EndMarker>(),
                    _ => throw new System.Exception("Invalid terrainId")
                };
                // Spawn the terrain prefab at the grid cell
                // The grid is flipped in the y-axis on the Unity plane, so inversion is required
                float invertedY = Clusterbyte.GRID_HEIGHT - y;
                GameObject spawned = toSpawn.SpawnAtGrid(x, invertedY);
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

    public bool IsOccupied(Vector3 position)
    {
        return takenPositions.Contains(Clusterbyte.ConvertTo2D(position));
    }

    public bool IsTerrain(Vector3 position)
    {
        return placeableTerrain.Any(t => Clusterbyte.ConvertTo2D(t.transform.position) == Clusterbyte.ConvertTo2D(position));
    }

    public void BuyItem(string itemName)
    {
        if (state != SHOPPING)
            return;

        if (IsOccupied(mouseHover.hoveredPosition))
        {
            Debug.Log("Cannot place item on an occupied tile.");
            return;
        }

        if (!EntityFactory.TryGet(itemName, out ShopDeployable item))
        {
            Debug.LogError($"Item {itemName} does not exist.");
            return;
        }

        Vector3 position = mouseHover.hoveredPosition;
        if (playerStats.TryTransaction(item.cost))
        {
            Debug.Log($"Bought {itemName} for {item.cost} tokens.");
            deployed.Add(Instantiate(item.prefab, position + new Vector3(0, 2, 0), Quaternion.identity));
            takenPositions.Add(Clusterbyte.ConvertTo2D(position));
            SetState(VIEWING);
        }
        else
        {
            Debug.Log("Not enough tokens to buy this item.");
        }
    }

    internal void Awake()
    {
        instance = this;
        TryGetComponent(out mouseHover);
        playerStats = GetComponent<PlayerStats>();
        playerStats.ResetStats();

        VIEWING = new GameState(ViewingInit, ViewingPeriodic, ViewingEnd);
        SHOPPING = new GameState(ShoppingInit, ShoppingPeriodic, ShoppingEnd);
        EDITING = new GameState(EditingInit, EditingPeriodic, EditingEnd);
        ACTIVE = new GameState(ActiveInit, ActivePeriodic, ActiveEnd);

        // Returns to overview setting at default or ESC
        SetState(VIEWING);
        ChangeStateOnEvent(() => state == SHOPPING && Input.GetKeyDown(KeyCode.Escape), VIEWING);
        ChangeStateOnEvent(() => state == EDITING && Input.GetKeyDown(KeyCode.Escape), VIEWING);

        // On click of terrain during overview setting, switch to observing states
        // ChangeStateOnEvent(() => state == SETTING && Input.GetMouseButtonDown(0) && mouseHover.isHovering && IsOccupied(mouseHover.hoveredPosition), EDITING);
        ChangeStateOnEvent(() => state == VIEWING && Input.GetMouseButtonDown(0) && mouseHover.isHovering, SHOPPING);
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
        for (int i = 0; i < Clusterbyte.ENEMY_SPAWN_TIMES[level].Length; i++)
        {
            Instantiate(navIndicatorPrefab, entitySpawn.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void ViewingInit()
    {
        statusText.text = $"Level {currentLevel + 1}";
        cam.interpolateSpeed = 2;
        cam.SetPosition(CameraPositioner.CameraSpot.ANGLED);
        mouseHover.enabled = true;
        viewUI.Show();
    }

    private void ViewingPeriodic()
    {
    }

    private void ViewingEnd()
    {
        viewUI.Hide();
    }

    private void ShoppingInit()
    {
        Vector3 worldClicked = mouseHover.hoveredPosition;
        cam.interpolateSpeed = 4;
        cam.SetObservingSpot(worldClicked + new Vector3(0, 3, -2), Quaternion.Euler(45, 0, 0));
        cam.SetPosition(CameraPositioner.CameraSpot.CUSTOM);
        shopUI.Show();
        mouseHover.enabled = false;
        statusText.text = $"Tile: ({worldClicked.x}, {worldClicked.z})";
    }

    private void ShoppingPeriodic()
    {
    }

    private void ShoppingEnd()
    {
        shopUI.Hide();
    }

    private void EditingInit()
    {
    }

    private void EditingPeriodic()
    {
    }

    private void EditingEnd()
    {
    }

    private void ActiveInit()
    {
        entitySpawn.SetActive(false);
        entityTarget.SetActive(false);
        cam.interpolateSpeed = 2;
        cam.SetPosition(CameraPositioner.CameraSpot.OVERHEAD);
        mouseHover.enabled = false;

        Tuple<float, string>[] wave = Clusterbyte.ENEMY_SPAWN_TIMES[currentLevel];
        foreach (Tuple<float, string> timePair in wave)
        {
            if (!EntityFactory.TryGet(timePair.Item2, out Entity.Entity e))
                continue;

            StartCoroutine(SpawnIn(timePair.Item1, e));
        }
    }

    private IEnumerator SpawnIn(float seconds, Entity.Entity toSpawn)
    {
        yield return new WaitForSeconds(seconds);
        Vector2 spawnPosition = Clusterbyte.ConvertTo2D(entitySpawn.transform.position);
        toSpawn.SpawnAtGrid(spawnPosition.x, spawnPosition.y);
    }

    private void ActivePeriodic()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) SetState(VIEWING);
        // if (playerStats.lives <= 0)
    }

    private void ActiveEnd()
    {
    }
}