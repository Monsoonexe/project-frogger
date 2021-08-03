using UnityEngine;

public class MovingPlatformTile : ATile
{
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

    public override void OnEnterTile()
    {
        base.OnEnterTile();
        var playerHandle = PlayerHost.Instance.PlayerMobileHandle;
        cachedParent = playerHandle.parent;

        playerHandle.SetParent(platformTransform);        
    }

    public override void OnExitTile()
    {
        base.OnEnterTile();
        //restore parent

        PlayerHost.Instance.PlayerMobileHandle.SetParent(cachedParent);
        cachedParent = null;
    }
}
