using UnityEngine;

/// <summary>
/// Handles interacting with tiles via raycast.
/// </summary>
public class TileDetector : ApexMonoBehaviour
{
    [SerializeField]
    private RaycastDetector raycaster;

    private void Reset()
    {
        //SetDevDescription("Handles ")
        raycaster = GetComponent<RaycastDetector>();
    }

    private void Awake()
    {
        raycaster.OnHitDetected += InterpretRaycastHit;
    }

    private void OnDestroy()
    {
        raycaster.OnHitDetected -= InterpretRaycastHit;
    }

    private void InterpretRaycastHit(in RaycastHit hitInfo)
    {
        if(hitInfo.collider.gameObject.TryGetComponent<ATile>(
            out ATile theTile))
        {
            theTile.OnEnterTile();
        }
    }
}
