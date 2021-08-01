using UnityEngine;

/// <summary>
/// Moves in a single direction and causes trouble for the player on collision.
/// </summary>
public class ProjectileHazard : ApexMobile
{
    private static PlayerHost playerHost;

    [Header("---Settings---")]
    public Vector3 moveVector = new Vector3(1, 0 , 0);

    [SerializeField]
    private float moveSpeed = 5;

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

    private void Update()
    {
        var moveStep = moveVector 
            * (moveSpeed * ApexGameController.DeltaTime);

        myTransform.Translate(moveStep);
    }
}
