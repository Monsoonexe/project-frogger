using UnityEngine;

public class SpawnRandomOnTimer : ASpawner
{
    [Header("---Settings---")]
    [SerializeField]
    private SpawnProfile[] objectPoolProfiles 
        = new SpawnProfile[0];//empty so null safe

    //runtime data
    private int profileIndex;

    public override void Init()
    {
        var len = objectPoolProfiles.Length;
        for(var i = 0; i < len; ++i)
        {
            var profile = objectPoolProfiles[i];

        }
        SpawnAfterDelay();
    }

    public override void SpawnAfterDelay()
    {
        //get local copy of profile
        var spawnProfile = objectPoolProfiles[profileIndex];

        //randomly select the next time to spawn
        var delay = spawnProfile.randomTimeIntervalBounds.RandomRange();

        //wait that much time, then spawn the thing
        ApexTweens.InvokeAfterDelay(SpawnObject, delay);
    }

    public override void SpawnObject()
    {
        //get new profile
        profileIndex = Random.Range(0, objectPoolProfiles.Length);//random element

        //get local copy of profile
        var spawnProfile = objectPoolProfiles[profileIndex];

        //peel profile
        var spawnPoint = spawnProfile.spawnPoint;
        var objectPool = spawnProfile.objectPool;
        var hazardLifetime = spawnProfile.hazardLifetime;

        //instantiate
        var newSpawn = objectPool.Depool(
            spawnPoint.position, spawnPoint.rotation);

        //reclaim after some time (unless immortal)
        if (newSpawn != null && hazardLifetime > 0)
            ApexTweens.InvokeAfterDelay(
                () => objectPool.Enpool(newSpawn), hazardLifetime);

        //chain forever (inlined SpawnAfterDelay() to decrease struct copies)
        //randomly select the next time to spawn
        var delay = spawnProfile.randomTimeIntervalBounds.RandomRange();

        //wait that much time, then call this method
        ApexTweens.InvokeAfterDelay(SpawnObject, delay);
    }
}
