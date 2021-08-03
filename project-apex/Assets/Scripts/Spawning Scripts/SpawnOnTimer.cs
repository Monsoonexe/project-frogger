using UnityEngine;

/// <summary>
/// Every x seconds, spawn a new hazard.
/// </summary>
public class SpawnOnTimer : ASpawner
{
    [Header("---Settings---")]
    [SerializeField]
    private Vector2 randomTimeIntervalBounds = new Vector2(1.5f, 4.0f);

    [SerializeField]
    [Tooltip("-1 means immortal.")]
    [Min(-1)]
    private float hazardLifetime = 10.0f;

    [Header("---Prefab Refs---")]
    [SerializeField]
    private Transform spawnPoint;

    private GameObjectPool objectPool;

    protected override void Reset()
    {
        SetDevDescription("I spawn items from a pool at time intervals.");
    }

    public override void Init()
    {
        objectPool = GetComponent<GameObjectPool>();
        SpawnAfterDelay();
    }

    public override void SpawnAfterDelay()
    {
        //randomly select the next time to spawn
        var randomDelay = randomTimeIntervalBounds.RandomRange();

        //wait that much time, then spawn the thing
        ApexTweens.InvokeAfterDelay(SpawnObject, randomDelay);
    }

    public override void SpawnObject()
    {
        SpawnAfterDelay();//chain forever

        //instantiate
        var newSpawn = objectPool.Depool(
            spawnPoint.position, spawnPoint.rotation);

        //reclaim after some time (unless immortal)
        if (newSpawn != null && hazardLifetime > 0)
            ApexTweens.InvokeAfterDelay(
                () => objectPool.Enpool(newSpawn), hazardLifetime);
    }

}
