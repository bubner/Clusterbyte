using UnityEngine;

public static class Clusterbyte
{
    private const int GRID_WIDTH = 18;
    private const int GRID_HEIGHT = 10;
    private const float WORLD_WIDTH = 18f;
    private const float WORLD_HEIGHT = 10f;
    private const float WORLD_LEFT = -9f;
    private const float WORLD_BOTTOM = -5f;

    // TODO: scale camera to fit the grid

    public static Vector2 SpawnAtTile(int x, int y)
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
}