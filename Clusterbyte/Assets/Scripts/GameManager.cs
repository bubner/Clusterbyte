using Lib;
using UnityEngine;
using Viewport;

public class GameManager : StateManager
{
    public static GameManager instance;

    [SerializeField] private GameObject terrainBlockPrefab;
    [SerializeField] private CameraPositioner cam;
    
    private MouseHover mouseHover;
    private CameraPositioner mainCamera;

    public static GameState SETTING;
    public static GameState OBSERVING;
    public static GameState ACTIVE;

    private void RegenerateField()
    {
        // TODO: more aware generation for internal state tracking of gameobjects on these squares
        for (int i = 1; i <= Clusterbyte.GRID_WIDTH; i++)
        {
            for (int j = 1; j <= Clusterbyte.GRID_HEIGHT; j++)
            {
                // Use Perlin noise to spawn procedurally
                // This will ensure that the terrain is not too clustered
                bool shouldSpawn = Mathf.PerlinNoise(i * Random.value, j * Random.value) > 0.5f;
                if (shouldSpawn)
                {
                    Clusterbyte.SpawnAtTile(i, j, terrainBlockPrefab, true);
                }
            }
        }
    }

    internal void Awake()
    {
        instance = this;
        Clusterbyte.ResetTerrainOccupiedCoordinates();
        TryGetComponent(out mouseHover);

        SETTING = new GameState(SettingInit, SettingPeriodic, SettingEnd);
        OBSERVING = new GameState(ObservingInit, ObservingPeriodic, ObservingEnd);
        ACTIVE = new GameState(ActiveInit, ActivePeriodic, ActiveEnd);

        // TODO: states for on game done for new field stuff
        RegenerateField();

        SetState(SETTING);
        ChangeStateOnEvent(() => state == SETTING && Input.GetMouseButtonDown(0) && mouseHover.isHovering, OBSERVING);
    }

    private void SettingInit()
    {
        cam.interpolateSpeed = 2;
        cam.SetPosition(CameraPositioner.CameraSpot.SETTING);
    }

    private void SettingPeriodic()
    {
    }

    private void SettingEnd()
    {
    }

    private void ObservingInit()
    {
        Vector2 worldClicked = mouseHover.worldHoveredPosition;
        cam.interpolateSpeed = 4;
        cam.SetObservingSpot(Clusterbyte.ConvertTo3D(worldClicked) + new Vector3(0, 3, -2), Quaternion.Euler(45, 0, 0));
        cam.SetPosition(CameraPositioner.CameraSpot.OBSERVING);
    }

    private void ObservingPeriodic()
    {
    }

    private void ObservingEnd()
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