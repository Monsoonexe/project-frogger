using UnityEngine;

/// <summary>
/// Moves in a single direction and causes trouble for the player on collision.
/// </summary>
public class ProjectileHazard : ApexMobile
{
    [Header("---Settings---")]
    [SerializeField]
    private float moveSpeed = 5;

    public void OnCollideWithPlayer()
    {
        PlayerHost.Instance.KillPlayer();
    }

    private void Update()
    {
        var moveStep = myTransform.forward 
            * (moveSpeed * ApexGameController.DeltaTime);

        myTransform.position += moveStep;
    }
}
