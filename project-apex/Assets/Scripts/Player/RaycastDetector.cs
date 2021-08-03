using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public delegate void RaycastListener(in RaycastHit hitInfo);

public class RaycastDetector : ApexMonoBehaviour
{
    [Header("---Settings---")]
    [SerializeField]
    protected bool detectOnRepeat = false;
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

    [Min(0)]
    [Tooltip("Note: modifying during playmode has no effect.")]
    public float raycastPollInterval = 0.2f;

    [Min(0)]
    public float detectDistance = 0.5f;

    /// <summary>
    /// What should this raycast collide with?
    /// </summary>
    [Tooltip("What should this raycast collide with?")]
    public LayerMask raycastLayerMask = -1;//everything

    /// <summary>
    /// Which layer is being detected? Facilitates querrying multiple layers.
    /// </summary>
    [Tooltip("Which layer is being detected? Facilitates querrying multiple layers.")]
    public LayerMask detectLayerMask = 4096; //hazard = 12

    public QueryTriggerInteraction detectTriggers
        = QueryTriggerInteraction.Ignore;

    public Vector3 detectVector = new Vector3(0, -1, 0);

    [Header("---Prefab Refs---")]
    public Transform raycastOriginPoint;

    [Header("---Events---")]
    [SerializeField]
    protected UnityEvent onDetected = new UnityEvent();

    public UnityEvent OnDetected { get => onDetected; }

    public event RaycastListener OnHitDetected;

    //runtime data
    protected YieldInstruction yieldInterval;

    protected void Reset()
    {
        SetDevDescription("I raise an event on a successful raycast hit!");
        raycastOriginPoint = GetComponent<Transform>();
    }

    private void Start()
    {
        yieldInterval = new WaitForSeconds(raycastPollInterval);
        OnHitDetected += RaiseUnityEvent;//
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
    /// Used to rig unity event to Action with arg so never null.
    /// </summary>
    /// <param name="disregardedValue"></param>
    protected void RaiseUnityEvent(in RaycastHit disregardedValue)
        => onDetected.Invoke();

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
                OnHitDetected(hitInfo);
            }
        }
    }

    protected IEnumerator RaycastRoutine()
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
