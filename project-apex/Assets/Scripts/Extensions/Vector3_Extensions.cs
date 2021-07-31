//credit to Jason Weimann and Unity College
using UnityEngine;

/// <summary>
/// 
/// </summary>
public static class Vector3_Extensions
{
    public static Vector3 WithX(this Vector3 a, float x)
        => new Vector3(x, a.y, a.z );

    public static Vector3 WithY(this Vector3 a, float y)
        => new Vector3(a.x, y, a.z);

    public static Vector3 WithZ(this Vector3 a, float z)
        => new Vector3(a.x, a.y, z);
}
