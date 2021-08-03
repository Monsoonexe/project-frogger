using UnityEngine;

/// <summary>
/// Base class for spawners.
/// </summary>
public abstract class ASpawner : ApexMonoBehaviour
{
    [Header("--Settings---")]
    public bool initOnAwake = true;

    protected virtual void Reset()
    {
        SetDevDescription("I spawn items.");
    }

    protected virtual void Awake()
    {
        if(initOnAwake)
            Init();
    }

    public abstract void Init();
    public abstract void SpawnAfterDelay();
    public abstract void SpawnObject();
}
