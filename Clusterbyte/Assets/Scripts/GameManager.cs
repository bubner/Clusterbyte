using System.Collections.Generic;
using System.Linq;
using Entity;
using Lib;
using Player;
using UnityEngine;
using Viewport;

public class GameManager : StateManager
{
    public static GameManager instance;
    public int level;

    [SerializeField] private CameraPositioner cam;
    [SerializeField] private UIExtensible observationUI;

    private MouseHover mouseHover;
    private PlayerStats playerStats;
    private CameraPositioner mainCamera;

    private readonly List<GameObject> placeableTerrain = new();
    private readonly List<GameObject> deployed = new();
    private readonly List<Vector2> takenPositions = new();

    public static GameState SETTING;
    public static GameState OBSERVING;
    public static GameState EDITING;
    public static GameState ACTIVE;

    /// <summary>
    /// Parses the current level map and spawns the terrain prefabs accordingly.
    /// </summary>
    private void ParseCurrentLevelMap()
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
                // Placeable terrain has id=2, inactive terrain has id=1
                Entity.Entity toSpawn = terrainId == 2 ? EntityFactory.Get<PlaceableTerrain>() : EntityFactory.Get<InactiveTerrain>();
                // Spawn the terrain prefab at the grid cell
                // The grid is flipped in the y-axis on the Unity plane, so inversion is required
                float invertedY = Clusterbyte.GRID_HEIGHT - y;
                GameObject spawned = toSpawn.SpawnAtGrid(x, invertedY);
                // Must also manage the list of terrain objects that the user can place on for validation
                if (terrainId == 2)
                    placeableTerrain.Add(spawned);
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
        if (state != OBSERVING)
            return;

        if (!EntityFactory.TryGet(itemName, out ShopDeployable item))
            return;

        if (playerStats.TryTransaction(item.cost))
        {
            Debug.Log($"Bought {itemName} for {item.cost} tokens.");
            Vector3 position = mouseHover.hoveredPosition;
            deployed.Add(Instantiate(item.prefab, position + new Vector3(0, 2, 0), Quaternion.identity));
            takenPositions.Add(Clusterbyte.ConvertTo2D(position));
        }
        else
        {
            Debug.Log("Not enough tokens to buy this item.");
        }

        SetState(SETTING);
    }

    internal void Awake()
    {
        instance = this;
        TryGetComponent(out mouseHover);
        TryGetComponent(out playerStats);
        playerStats.ResetTokens();

        SETTING = new GameState(SettingInit, SettingPeriodic, SettingEnd);
        OBSERVING = new GameState(ObservingInit, ObservingPeriodic, ObservingEnd);
        EDITING = new GameState(EditingInit, EditingPeriodic, EditingEnd);
        ACTIVE = new GameState(ActiveInit, ActivePeriodic, ActiveEnd);

        // Returns to overview setting at default or ESC
        SetState(SETTING);
        ChangeStateOnEvent(() => state == OBSERVING && Input.GetKeyDown(KeyCode.Escape), SETTING);
        ChangeStateOnEvent(() => state == EDITING && Input.GetKeyDown(KeyCode.Escape), SETTING);

        // On click of terrain during overview setting, switch to observing states
        // ChangeStateOnEvent(() => state == SETTING && Input.GetMouseButtonDown(0) && mouseHover.isHovering && IsOccupied(mouseHover.hoveredPosition), EDITING);
        ChangeStateOnEvent(() => state == SETTING && Input.GetMouseButtonDown(0) && mouseHover.isHovering, OBSERVING);
    }

    internal void Start()
    {
        // TODO: states for on game done for new field stuff
        ParseCurrentLevelMap();
    }

    private void SettingInit()
    {
        cam.interpolateSpeed = 2;
        cam.SetPosition(CameraPositioner.CameraSpot.SETTING);
        mouseHover.enabled = true;
    }

    private void SettingPeriodic()
    {
    }

    private void SettingEnd()
    {
    }

    private void ObservingInit()
    {
        Vector3 worldClicked = mouseHover.hoveredPosition;
        cam.interpolateSpeed = 4;
        cam.SetObservingSpot(worldClicked + new Vector3(0, 3, -2), Quaternion.Euler(45, 0, 0));
        cam.SetPosition(CameraPositioner.CameraSpot.OBSERVING);
        observationUI.Show();
        mouseHover.enabled = false;
    }

    private void ObservingPeriodic()
    {
    }

    private void ObservingEnd()
    {
        observationUI.Hide();
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
    }

    private void ActivePeriodic()
    {
    }

    private void ActiveEnd()
    {
    }
}