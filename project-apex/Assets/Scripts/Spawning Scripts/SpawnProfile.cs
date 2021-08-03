using System;
using UnityEngine;

[Serializable]
public struct SpawnProfile
{
    [Min(-1)]
    public float hazardLifetime;
    public Vector2 randomTimeIntervalBounds;
    public Transform spawnPoint;
    public GameObjectPool objectPool;
}
