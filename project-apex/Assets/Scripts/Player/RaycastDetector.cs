using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RaycastDetector : ApexMonoBehaviour
{
    [Header("---Settings---")]
    [SerializeField]
    [Tooltip("Note: modifying during playmode has no effect.")]
    private float raycastPollInterval = 0.2f;

    [SerializeField]
    [Min(0)]
    private float detectDistance = 0.5f;

    [SerializeField]
    private LayerMask raycastLayerMask = -1;//everything

    [SerializeField]
    private QueryTriggerInteraction detectTriggers
        = QueryTriggerInteraction.Ignore;

    [SerializeField]
    private Vector3 detectVector = new Vector3(0, -1, 0);

    [Header("---Prefab Refs---")]
    [SerializeField]
    private Transform raycastOriginPoint;

    [Header("---Events---")]
    [SerializeField]
    private UnityEvent onDetected = new UnityEvent();

    //runtime data
    private YieldInstruction yieldInterval;

    private void Reset()
    {
        SetDevDescription("I raise an event on a successful raycast hit!");
        raycastOriginPoint = GetComponent<Transform>();
    }

    private void Start()
    {
        yieldInterval = new WaitForSeconds(raycastPollInterval);
    }

    private void OnEnable()
    {
        StartCoroutine(RaycastRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void HandleRayDetection()
    {
        //grounded if hit something on ground layer
        var detectedHazard = Physics.Raycast(
            raycastOriginPoint.position, detectVector,
            out RaycastHit hitInfo,
            detectDistance, raycastLayerMask,
            detectTriggers);

        if (detectedHazard)
            onDetected.Invoke();
    }

    private IEnumerator RaycastRoutine()
    {
        //infinite loop (stopped OnDisable())
        var RichIsAwesome = true;
        while(RichIsAwesome)
        {
            HandleRayDetection();
            yield return yieldInterval;
        }
    }
}
