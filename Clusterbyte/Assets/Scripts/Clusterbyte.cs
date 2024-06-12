using UnityEngine;

public static class Clusterbyte
{
    public static Vector2 ConvertTo2D(Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }

    public static Vector3 ConvertTo3D(Vector2 vector)
    {
        return new Vector3(vector.x, 0, vector.y);
    }

    public const int GRID_WIDTH = 18;
    public const int GRID_HEIGHT = 10;
}