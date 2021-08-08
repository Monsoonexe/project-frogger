using UnityEngine;

/// <summary>
/// Every x seconds, spawn a new hazard.
/// </summary>
[RequireComponent(typeof(Timer))]
[RequireComponent(typeof(GameObjectPool))]
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

    //member Components
    private GameObjectPool objectPool;
    private Timer spawnTimer;

    protected override void Reset()
    {
        SetDevDescription("I spawn items from a pool at " +
            "time intervals.");
    }

    public override void Init()
    {
        spawnTimer = GetComponent<Timer>();
        objectPool = GetComponent<GameObjectPool>();

        //init timer
        spawnTimer.OnTimerExpire.AddListener(SpawnObject);

        //kick off
        SpawnAfterDelay();
    }

    public override void SpawnAfterDelay()
    {
        //randomly select the next time to spawn
        var randomDelay = randomTimeIntervalBounds.RandomRange();

        //wait that much time, then spawn the thing
        //ApexTweens.InvokeAfterDelay(SpawnObject, randomDelay); //tween that requires passing func*
        spawnTimer.Initialize(randomDelay); //pre-init'd timer (saves 120 Bytes of GC compared to ^^^
    }

    public override void SpawnObject()
    {
        SpawnAfterDelay();//chain forever

        //instantiate
        var newSpawn = objectPool.Depool(
            spawnPoint.position, spawnPoint.rotation);

        void EnpoolAfterLifetime()
        {   //this local function is a closure, so generating Bytes on creation makes sense.
            objectPool.Enpool(newSpawn); //but why does this line cost 56 B of GC.Alloc?
        }

        //not practical to use a Timer here as there would need to be 
        //one Timer per spawned object, and that would require another object pool
        //just eat the ~100 bytes every ~7 seconds

        //reclaim after some time (unless immortal)
        if (newSpawn != null && hazardLifetime > 0)
            ApexTweens.InvokeAfterDelay(
                EnpoolAfterLifetime, hazardLifetime);
    }
}
