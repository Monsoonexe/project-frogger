using UnityEngine;

public static class Vector2_Extensions
{
    public static float RandomRange(this Vector2 range)
        => Random.Range(range.x, range.y);

    /// <summary>
    /// Returns true if x is in inverval [range.x, range.y].
    /// </summary>
    public static bool IsWithin(this Vector2 range, float x)
        => (x >= range.x && x <= range.y);

    /// <summary>
    /// Returns true if x is in inverval [range.x, range.y].
    /// </summary>
    public static bool IsWithin(this Vector2Int range, int x)
        => (x >= range.x && x <= range.y);
}