using System.Collections;
using UnityEngine;

/// <summary>
/// More like "sticky" platform.
/// </summary>
public class MovingPlatformTile : ATile
{
    [Header("---Settings---")]
    [SerializeField]
    private bool snapPlayerToCenter = false;

    [SerializeField]
    private float snapSpeed = 1.0f;

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
        //TODO guard this better instead of every frame
        //move player towards center point of object
        if(CurrentTile == this && snapPlayerToCenter)
        {
        }
    }

    private IEnumerator MovePlayerToCenter()
    {
        const float duration = 1.0f; //only do this for 1 second
        float runtime = 0.0f;
        var playerHandle = PlayerHost.Instance.PlayerMobileHandle;
        var currentPosition = playerHandle.localPosition;
        var anchorPoint = Vector3.zero
            .WithY(currentPosition.y);
        do
        {
            var deltaTime = ApexGameController.DeltaTime;
            playerHandle.localPosition = Vector3.MoveTowards(
                currentPosition, anchorPoint,
                 deltaTime * snapSpeed);

            runtime += deltaTime;
            yield return null;//next frame

        } while (runtime < duration);
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
