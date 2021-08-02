using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RaycastDetector : ApexMonoBehaviour
{
    [Header("---Settings---")]
    [SerializeField]
    private bool detectOnRepeat = false;
    public bool DetectOnRepeat
    {
        get => detectOnRepeat;
        set
        {
            //if was off and now on
            if (value == true && detectOnRepeat == false)
                StartCoroutine(RaycastRoutine());
            detectOnRepeat = value;
        }
    }

    [SerializeField]
    [Tooltip("Note: modifying during playmode has no effect.")]
    private float raycastPollInterval = 0.2f;

    [SerializeField]
    [Min(0)]
    private float detectDistance = 0.5f;

    /// <summary>
    /// What should this raycast collide with?
    /// </summary>
    [SerializeField]
    [Tooltip("What should this raycast collide with?")]
    private LayerMask raycastLayerMask = -1;//everything

    /// <summary>
    /// Which layer is being detected? Facilitates querrying multiple layers.
    /// </summary>
    [SerializeField]
    [Tooltip("Which layer is being detected? Facilitates querrying multiple layers.")]
    private LayerMask detectLayerMask = 4096; //hazard = 12

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
        if(detectOnRepeat)
            StartCoroutine(RaycastRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// Raise an event when something is hit and is on given layer mask.
    /// </summary>
    public void HandleRayDetection()
    {
        //grounded if hit something on ground layer
        bool rayHitSomething = Physics.Raycast(
            raycastOriginPoint.position, detectVector,
            out RaycastHit hitInfo,
            detectDistance, raycastLayerMask,
            detectTriggers);

        if (rayHitSomething)
        {
            //layer returns 0-31, we need a LayerMask, not a bit number
            int hitLayerBitNumber = hitInfo.collider.gameObject.layer;
            LayerMask hitLayer = (1 << hitLayerBitNumber);//int 2 mask
            //bitshifting has same behaviour as code below:
            //(int)Mathf.Pow(2, hitLayerBitNumber);//int 2 mask
            uint maskResult = (uint)(detectLayerMask & hitLayer);
            //check if that something is on the hazard layer
            if (maskResult > 0)
            {
                onDetected.Invoke();
            }
        }
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
