using System;
using UnityEngine;

/// <summary>
/// Level generation and other constants for Clusterbyte.
/// </summary>
public static class Clusterbyte
{
    /// <summary>
    /// Convert a world position to a grid position.
    /// </summary>
    /// <param name="vector">world vector</param>
    /// <returns>grid vector</returns>
    public static Vector2 ConvertTo2D(Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }

    /// <summary>
    /// Convert a grid position to a world position.
    /// </summary>
    /// <param name="vector">grid vector</param>
    /// <returns>world vector</returns>
    public static Vector3 ConvertTo3D(Vector2 vector)
    {
        return new Vector3(vector.x, 0, vector.y);
    }

    /// <summary>
    /// The width of the grid in units.
    /// </summary>
    public const int GRID_WIDTH = 18;
    /// <summary>
    /// The height of the grid in units.
    /// </summary>
    public const int GRID_HEIGHT = 10;
    /// <summary>
    /// The distance threshold to consider the enemy path as reached.
    /// </summary>
    public const float END_OF_PATH_TRIGGER_BOX_THRESHOLD = 0.5f;

    /// <summary>
    /// Level terrain data.
    /// </summary>
    public static readonly int[][][] LEVELS_TERRAIN =
    {
        new []
        {
            new [] { 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 1 },
            new [] { 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1 },
            new [] { 1, 0, 2, 1, 1, 0, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 1 },
            new [] { 1, 0, 1, 1, 1, 0, 2, 1, 1, 1, 0, 1, 0, 2, 1, 1, 0, 1 },
            new [] { 1, 0, 1, 1, 1, 0, 2, 1, 1, 2, 0, 1, 0, 2, 1, 2, 0, 1 },
            new [] { 1, 0, 2, 1, 1, 0, 2, 1, 1, 2, 0, 1, 0, 1, 1, 2, 0, 1 },
            new [] { 1, 0, 2, 1, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 0, 1 },
            new [] { 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 1, 0, 1 },
            new [] { 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1 },
            new [] { 1, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        }
    };

    /// <summary>
    /// Enemy spawn data.
    /// </summary>
    public static readonly Tuple<float, string>[][] ENEMY_SPAWN_TIMES =
    {
        new[]
        {
            new Tuple<float, string>(0.1f, "Blob"),
            new(1f, "Blob"),
            new(3f, "Blob"),
            new(4f, "Blob"),
            new(6f, "Blob"),
        },
    };
}