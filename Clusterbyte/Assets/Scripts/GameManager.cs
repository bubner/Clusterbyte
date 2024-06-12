using System.Collections.Generic;
using System.Linq;
using Entity;
using Entity.Placeables;
using Lib;
using Player;
using UnityEngine;
using Viewport;
using Terrain = Entity.Terrain;

public class GameManager : StateManager
{
    public static GameManager instance;

    [SerializeField] private CameraPositioner cam;
    [SerializeField] private UIExtensible observationUI;

    private MouseHover mouseHover;
    private PlayerStats playerStats;
    private CameraPositioner mainCamera;

    private readonly List<Vector3> terrain = new();
    private readonly List<GameObject> deployed = new();
    private readonly List<Vector2> takenPositions = new();

    public static GameState SETTING;
    public static GameState OBSERVING;
    public static GameState EDITING;
    public static GameState ACTIVE;

    private void RegenerateField()
    {
        for (int i = 0; i <= Clusterbyte.GRID_WIDTH; i++)
        {
            for (int j = 0; j <= Clusterbyte.GRID_HEIGHT; j++)
            {
                // Use Perlin noise to spawn procedurally
                // This will ensure that the terrain is not too clustered
                bool shouldSpawn = Mathf.PerlinNoise(i * Random.value, j * Random.value) > 0.5f;
                if (!shouldSpawn)
                    continue;
                EntityFactory.Get<Terrain>().SpawnAtGrid(i, j);
                terrain.Add(Clusterbyte.ConvertTo3D(new Vector2(i, j)));
            }
        }
    }

    public bool IsOccupied(Vector3 position)
    {
        return takenPositions.Contains(Clusterbyte.ConvertTo2D(position));
    }

    public bool IsTerrain(Vector3 position)
    {
        return terrain.Contains(position);
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

        // TODO: states for on game done for new field stuff
        RegenerateField();

        SetState(SETTING);
        ChangeStateOnEvent(() => state == SETTING && Input.GetMouseButtonDown(0) && mouseHover.isHovering && IsOccupied(mouseHover.hoveredPosition), EDITING);
        // TODO: Setting Mode
        // ChangeStateOnEvent(() => state == OBSERVING && Input.GetKeyDown(KeyCode.Escape), SETTING);
        // ChangeStateOnEvent(() => state == EDITING && Input.GetKeyDown(KeyCode.Escape), SETTING);
        ChangeStateOnEvent(() => state == SETTING && Input.GetMouseButtonDown(0) && mouseHover.isHovering, OBSERVING);
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