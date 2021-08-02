using UnityEngine;

/// <summary>
/// Moves in a single direction and causes trouble for the player on collision.
/// </summary>
public class ProjectileHazard : ApexMobile
{
    private static PlayerHost playerHost;

    [Header("---Settings---")]
    [SerializeField]
    private float moveSpeed = 5;

    [Header("---Audio---")]
    [SerializeField]
    private AudioClip onSpawnClip;

    protected override void Awake()
    {
        base.Awake();
        if (!playerHost)
            playerHost = PlayerHost.Instance;
    }

    public void OnCollideWithPlayer()
    {
        playerHost.KillPlayer();
    }

    private void OnEnable()
    {
        onSpawnClip.Play();//bang!
    }

    private void Update()
    {
        var moveStep = myTransform.forward 
            * (moveSpeed * ApexGameController.DeltaTime);

        myTransform.position += moveStep;
    }
}
