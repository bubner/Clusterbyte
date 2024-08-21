using System;
using UnityEngine;

/// <summary>
/// Level generation and other constants for Clusterbyte.
/// </summary>
public static class Clusterbyte
{
    /// <summary>
    /// The next level that should be queued to run. Nullable.
    /// </summary>
    public static int? QUEUED_LEVEL = null;

    /// <summary>
    /// Number of lives for a new save.
    /// </summary>
    public const int STARTING_LIVES = 25;

    /// <summary>
    /// Convert a world position to a grid position.
    /// </summary>
    /// <param name="vector">world vector</param>
    /// <returns>grid vector</returns>
    public static Vector2 ConvertTo2D(Vector3 vector)
    {
        // Need to round to the nearest integer to avoid floating point errors
        return new Vector2(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.z));
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
        },
        new []
        {
            new [] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            new [] { 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1 },
            new [] { 1, 1, 1, 0, 2, 1, 1, 1, 2, 2, 1, 1, 0, 0, 2, 1, 1, 1 },
            new [] { 1, 2, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1 },
            new [] { 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 2, 0, 1, 1 },
            new [] { 1, 2, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1 },
            new [] { 1, 2, 0, 1, 1, 1, 1, 1, 1, 0, 2, 0, 1, 1, 1, 1, 1, 1 },
            new [] { 1, 1, 0, 2, 2, 2, 1, 1, 2, 0, 0, 0, 0, 0, 0, 0, 2, 1 },
            new [] { 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 1, 1 },
            new [] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 1 },
        },
        new []
        {
            new [] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            new [] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 2, 0, 0, 0, 1 },
            new [] { 3, 0, 0, 0, 0, 1, 1, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 1 },
            new [] { 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 0, 1, 1, 0, 1, 0, 1 },
            new [] { 1, 2, 2, 1, 0, 1, 1, 0, 0, 2, 0, 0, 1, 1, 0, 1, 0, 1 },
            new [] { 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 0, 2, 1, 0, 1, 0, 1 },
            new [] { 1, 0, 0, 0, 0, 1, 1, 0, 0, 2, 0, 0, 2, 0, 0, 1, 0, 1 },
            new [] { 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 0, 1, 0, 1 },
            new [] { 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 0, 1 },
            new [] { 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 1 },
        },
        new []
        {
            new [] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            new [] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            new [] { 1, 0, 2, 1, 1, 1, 1, 2, 1, 2, 1, 2, 1, 1, 1, 1, 0, 1 },
            new [] { 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1 },
            new [] { 1, 0, 1, 0, 1, 1, 1, 2, 1, 2, 1, 2, 1, 1, 0, 1, 0, 1 },
            new [] { 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0, 1 },
            new [] { 1, 0, 1, 0, 1, 4, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 0, 1 },
            new [] { 1, 0, 1, 0, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 2, 0, 1 },
            new [] { 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            new [] { 1, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        },
        new []
        {
            new [] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
            new [] { 2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2 },
            new [] { 2, 0, 2, 0, 2, 0, 2, 2, 0, 0, 2, 0, 2, 0, 2, 2, 0, 2 },
            new [] { 2, 0, 2, 0, 2, 0, 0, 2, 0, 0, 2, 0, 2, 0, 2, 2, 0, 2 },
            new [] { 2, 0, 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 2 },
            new [] { 2, 0, 2, 0, 2, 3, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 2 },
            new [] { 2, 0, 2, 0, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 2, 0, 2 },
            new [] { 2, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 2, 0, 2, 0, 2, 0, 2 },
            new [] { 2, 0, 2, 2, 2, 2, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0, 2 },
            new [] { 2, 4, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
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
            new(7f, "Blob"),
            new(8f, "Blob"),
            new(9f, "Blob"),
            new(10f, "Blob"),
        },
        new[]
        {
            new Tuple<float, string>(0.1f, "Drone"),
            new(1f, "Blob"),
            new(2f, "Blob"),
            new(3f, "Blob"),
            new(4f, "Blob"),
            new(6f, "Drone"),
            new(6.3f, "Drone"),
            new(6.6f, "Drone"),
            new(6.9f, "Drone"),
        },
        new[]
        {
            new Tuple<float, string>(0.1f, "Drone"),
            new(1f, "Drone"),
            new(2f, "Drone"),
            new(3f, "Drone"),
            new(4f, "Drone"),
            new(5f, "Drone"),
            new(6f, "Drone"),
            new(7f, "Drone"),
            new(9f, "Zoomer"),
            new(9.1f, "Zoomer"),
            new(9.2f, "Zoomer"),
            new(9.3f, "Zoomer"),
            new(9.4f, "Zoomer"),
            new(9.5f, "Zoomer"),
            new(9.6f, "Zoomer"),
            new(9.7f, "Zoomer"),
            new(9.8f, "Zoomer"),
            new(9.9f, "Zoomer"),
            new(10f, "Zoomer"),
            new(10.1f, "Blob"),
            new(10.5f, "Blob"),
            new(11f, "Blob"),
            new(11.5f, "Blob"),
            new(12f, "Blob"),
            new(12.5f, "Blob"),
        },
        new []
        {
            new Tuple<float, string>(0.1f, "Zoomer"),
            new(0.2f, "Zoomer"),
            new(0.3f, "Zoomer"),
            new(0.4f, "Zoomer"),
            new(0.5f, "Zoomer"),
            new(0.6f, "Zoomer"),
            new(0.7f, "Zoomer"),
            new(0.8f, "Zoomer"),
            new(3.1f, "Drone"),
            new(3.6f, "Drone"),
            new(3.9f, "Drone"),
            new(4.2f, "Drone"),
            new(6f, "Blob"),
            new(6.2f, "Blob"),
            new(6.4f, "Blob"),
            new(6.6f, "Blob"),
            new(7f, "Blob"),
            new(10, "Tank"),
            new(10.1f, "Zoomer"),
            new(11, "Tank"),
            new(11.1f, "Zoomer"),
            new(16, "Tank"),
            new(16.1f, "Zoomer"),
            new(17, "Tank"),
            new(17.1f, "Zoomer"),
            new(18, "Tank"),
            new(18.1f, "Zoomer"),
            new(19, "Tank"),
            new(19.1f, "Zoomer"),
            new(19.8f, "Tank"),
        },
        new []
        {
            new Tuple<float, string>(0.7f, "Drone"),
            new(1.4f, "Drone"),
            new(2.1f, "Drone"),
            new(2.8f, "Drone"),
            new(3.5f, "Drone"),
            new(4.2f, "Drone"),
            new(4.9f, "Drone"),
            new(5.6f, "Drone"),
            new(6.3f, "Drone"),
            new(7.0f, "Drone"),
            new(7.7f, "Drone"),
            new(8.4f, "Drone"),
            new(9.1f, "Drone"),
            new(9.8f, "Drone"),
            new(10.5f, "Drone"),
            new(11.2f, "Drone"),
            new(11.9f, "Drone"),
            new(12.6f, "Drone"),
            new(13.3f, "Drone"),
            new(14.0f, "Drone"),
            new(16.0f, "Zoomer"),
            new(16.4f, "Zoomer"),
            new(16.8f, "Zoomer"),
            new(17.2f, "Zoomer"),
            new(17.6f, "Zoomer"),
            new(18.0f, "Zoomer"),
            new(18.4f, "Zoomer"),
            new(18.8f, "Zoomer"),
            new(19.2f, "Zoomer"),
            new(19.6f, "Zoomer"),
            new(20.0f, "Zoomer"),
            new(20.4f, "Zoomer"),
            new(20.8f, "Zoomer"),
            new(21.2f, "Zoomer"),
            new(21.6f, "Zoomer"),
            new(22.0f, "Zoomer"),
            new(22.4f, "Zoomer"),
            new(22.8f, "Zoomer"),
            new(23.2f, "Zoomer"),
            new(23.6f, "Zoomer"),
            new(25.0f, "Blob"),
            new(25.5f, "Tank"),
            new(26.0f, "Blob"),
            new(26.5f, "Tank"),
            new(27.0f, "Blob"),
            new(27.5f, "Tank"),
            new(32.0f, "Big Boy"),
            new(35.0f, "Big Boy"),
            new(37.0f, "Big Boy"),
            new(40.0f, "Big Boy"),
            new(43.0f, "Big Boy"),
            new(46.0f, "Big Boy"),
            new(49.0f, "Big Boy"),
            new(52.0f, "Big Boy"),
            new(55.0f, "Big Boy"),
            new(58.0f, "Big Boy")
        }
    };

    public static readonly int[] TOKENS_PER_LEVEL = { 15, 25, 35, 60, int.MaxValue };
}