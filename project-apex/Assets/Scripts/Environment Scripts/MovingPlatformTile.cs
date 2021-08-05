using UnityEngine;

/// <summary>
/// More like "sticky" platform.
/// </summary>
public class MovingPlatformTile : ATile
{
    [Header("---Settings---")]
    [SerializeField]
    private bool snapPlayerToCenter = false;

    [Tooltip("If snapping, how fast?")]
    public float snapSpeed = 1.0f;

    [Header("---Prefab Refs---")]
    [SerializeField]
    private Transform platformTransform;

    //runtime data
    private Transform cachedParent;

    private void Reset()
    {
        SetDevDescription("The Player will stick to me while touching it.");
        platformTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        if(CurrentTile == this && snapPlayerToCenter)
        {
            if (snapPlayerToCenter)
            {
                var playerHandle = PlayerHost.Instance.PlayerMobileHandle;
                var localPosition = playerHandle.localPosition;

                //move to center but preserve height
                var anchorPoint = Vector3.zero
                    .WithY(localPosition.y);

                var moveStep = Vector3.MoveTowards(
                    localPosition, anchorPoint,
                    ApexGameController.DeltaTime * snapSpeed);
                playerHandle.localPosition = moveStep;
            }
        }
    }

    public override void OnEnterTile()
    {
        base.OnEnterTile();
        var playerHandle = PlayerHost.Instance.PlayerMobileHandle;
        cachedParent = playerHandle.parent;

        playerHandle.SetParent(platformTransform);

        //handle snapping player
        //if (snapPlayerToCenter)
        //{
        //    var anchorPoint = Vector3.zero
        //        .WithY(playerHandle.localPosition.y);
        //    playerHandle.localPosition = anchorPoint;
        //}
    }

    public override void OnExitTile()
    {
        base.OnExitTile();
        //restore parent
        PlayerHost.Instance.PlayerMobileHandle.SetParent(cachedParent);
        cachedParent = null;
    }
}
