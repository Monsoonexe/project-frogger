using UnityEngine;

public class TileDetector : ApexMonoBehaviour
{
    [SerializeField]
    private RaycastDetector raycaster;

    private void Reset()
    {
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
