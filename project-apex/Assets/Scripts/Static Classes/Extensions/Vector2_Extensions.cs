using UnityEngine;

public static class Vector2_Extensions
{
    public static float RandomRange(this Vector2 range)
        => Random.Range(range.x, range.y);
}