using UnityEngine;

public static class Clusterbyte
{
    public const int GRID_WIDTH = 18;
    public const int GRID_HEIGHT = 10;
    public const float WORLD_WIDTH = 18f;
    public const float WORLD_HEIGHT = 10f;
    public const float WORLD_LEFT = -9f;
    public const float WORLD_BOTTOM = -5f;

    private static int[][] terrainOccupiedCoordinates;

    /// <summary>
    /// Convert world vectors into grid coordinates
    /// </summary>
    /// <param name="x">x starting from bottom left grid</param>
    /// <param name="y">y starting from bottom left of grid</param>
    /// <returns>A 2D vector representing the world vector of the grid</returns>
    /// <exception cref="System.Exception">invalid input</exception>
    public static Vector2 GridToWorld(int x, int y)
    {
        if (x < 1 || x > GRID_WIDTH || y < 1 || y > GRID_HEIGHT)
        {
            throw new System.Exception("Invalid tile number");
        }

        // Remap grid coordinates to world vectors
        float worldX = (x - 1) * (WORLD_WIDTH / (GRID_WIDTH - 1)) + WORLD_LEFT;
        float worldY = (y - 1) * (WORLD_HEIGHT / (GRID_HEIGHT - 1)) + WORLD_BOTTOM;

        return new Vector2(Mathf.RoundToInt(worldX), Mathf.RoundToInt(worldY));
    }

    /// <summary>
    /// Convert world vectors into grid coordinates
    /// </summary>
    /// <param name="x">x world vector (2d)</param>
    /// <param name="y">y world vector (2d)</param>
    /// <returns>Grid Vector</returns>
    /// <exception cref="System.Exception">invalid range</exception>
    public static Vector2 WorldToGrid(int x, int y)
    {
        if (x < WORLD_LEFT || x > WORLD_LEFT + WORLD_WIDTH || y < WORLD_BOTTOM || y > WORLD_BOTTOM + WORLD_HEIGHT)
        {
            throw new System.Exception("Invalid world coordinate");
        }

        // Remap world vectors to grid coordinates
        float gridX = (x - WORLD_LEFT) / (WORLD_WIDTH / (GRID_WIDTH - 1)) + 1;
        float gridY = (y - WORLD_BOTTOM) / (WORLD_HEIGHT / (GRID_HEIGHT - 1)) + 1;

        return new Vector2(Mathf.RoundToInt(gridX), Mathf.RoundToInt(gridY));
    }

    /// <summary>
    /// Convert a Unity world coordinate to a 2D Clusterbyte coordinate
    /// </summary>
    public static Vector2 ConvertTo2D(Vector3 pos)
    {
        return new Vector2(pos.x, pos.z);
    }

    /// <summary>
    /// Convert a 2D Clusterbyte coordinate to a Unity world coordinate
    /// </summary>
    public static Vector3 ConvertTo3D(Vector2 pos)
    {
        return new Vector3(pos.x, 0, pos.y);
    }

    public static void SpawnAtTile(int x, int y, GameObject toSpawn, bool isTerrain = false)
    {
        Vector2 position = GridToWorld(x, y);
        if (isTerrain)
            terrainOccupiedCoordinates[x - 1][y - 1] = 1;
        Object.Instantiate(toSpawn, new Vector3(position.x, 0.5f, position.y), Quaternion.identity);
    }

    public static void RemoveTerrainAtTile(int x, int y, GameObject obj)
    {
        if (terrainOccupiedCoordinates[x - 1][y - 1] == 0)
        {
            throw new System.Exception("No terrain to remove at this tile");
        }
        Object.Destroy(obj);
        terrainOccupiedCoordinates[x - 1][y - 1] = 0;
    }

    public static void ResetTerrainOccupiedCoordinates()
    {
        terrainOccupiedCoordinates = new int[GRID_WIDTH][];
        for (int i = 0; i < GRID_WIDTH; i++)
        {
            terrainOccupiedCoordinates[i] = new int[GRID_HEIGHT];
        }
    }

    public static bool IsTileOccupiedByTerrain(int x, int y)
    {
        if (x < 1 || x > GRID_WIDTH || y < 1 || y > GRID_HEIGHT)
        {
            return false;
        }
        return terrainOccupiedCoordinates[x - 1][y - 1] == 1;
    }
}