using UnityEngine;

/// <summary>
/// Handles Frogger-like movement.
/// </summary>
public class FroggerMobile : ApexMobile
{
    [Header("---Settings---")]
    [SerializeField]
    private Vector3 stepAmount = new Vector3(5, 0, 5);

    [Header("---Bounds Settings---")]
    [SerializeField]
    private Vector2 xBounds = new Vector2(-50, 50);

    [SerializeField]
    private Vector2 yBounds = new Vector2(1.5f, 1.5f);

    [SerializeField]
    private Vector2 zBounds = new Vector2(-100, 100);

    [SerializeField]
    private LayerMask boundaryCheckMask;

    [Header("---Animation Settings---")]
    [SerializeField]
    private float moveAnimTime = 0.85f;

    [Header("---Audio---")]
    [SerializeField]
    private AudioClip moveAudioClip;

    //[SerializeField]
    //[Tooltip("Controls are more responsive with a lower value, " +
    //    "but tween end might be cut off.")]
    //private float responseModifier = 0.9f;

    [Header("---Prefab Refs---")]
    [SerializeField]
    private Transform playerModelHandle;

    [SerializeField]
    private RaycastDetector tileRaycaster;

    //[SerializeField]
    //private Gravity gravityBehaviour;

    //runtime data
    private Coroutine moveTween;

    private void Reset()
    {
        SetDevDescription("The bit that actually moves around the world.");
        //gravityBehaviour = GetComponentInChildren<Gravity>();
        tileRaycaster = GetComponentInChildren<RaycastDetector>();
    }

    public bool IsMoving { get => moveTween != null; }

    public void Move(Vector3 input)
    {   //prevent spamming and moving too frequently
        if (IsMoving    //don't animate to zero
            || (input.x == 0 && input.z == 0))
            return;

        //calculate target destination for lerp
        var forwardMove = input.z * stepAmount.z;

        //prioritize forward movement over horizontal.
        var horizontalMove = (forwardMove == 0) 
            ? input.x * stepAmount.x : 0;

        var desiredDestination = new Vector3(horizontalMove, 0, forwardMove)
            + playerModelHandle.position;

        //TODO instead use a raycast so can use complex colliders
        //instead of custom math

        //local function to handle end-of-animation stuff
        void OnMoveComplete()
        {
            moveTween = null; //flag not in use
            tileRaycaster.HandleRayDetection(); //this class handles what to do on hit
            //gravityBehaviour.enabled = true;
        }

        //if any of the values are outside of boundary, no movement at all
        if (xBounds.IsWithin(desiredDestination.x)
            && yBounds.IsWithin(desiredDestination.y)
            && zBounds.IsWithin(desiredDestination.z))
        {
            //disable gravity during movement.
            //gravityBehaviour.enabled = false;
            moveAudioClip.Play();
            moveTween = playerModelHandle.Tween_Lerp(
                desiredDestination, moveAnimTime,
                onComplete: OnMoveComplete);
        }
    }

    /// <summary>
    /// Returns true if x is on inverval [min, max].
    /// </summary>
    public static bool IsWithin(int x, int min, int max)
        => (x >= min && x <= max);

    /// <summary>
    /// Returns true if x is on inverval [min, max].
    /// </summary>
    public static bool IsWithin(float x, float min, float max)
        => (x >= min && x <= max);
}
