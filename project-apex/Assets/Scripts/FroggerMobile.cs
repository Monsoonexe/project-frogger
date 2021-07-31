using UnityEngine;
using System.Collections;

public class FroggerMobile : ApexMobile
{
    [Header("---Settings---")]
    [SerializeField]
    private Vector3 stepAmount = new Vector3(5, 0, 5);

    [Header("---Animation Settings---")]
    [SerializeField]
    private float moveAnimTime = 0.85f;

    [SerializeField]
    [Tooltip("Controls are more responsive with a lower value, " +
        "but tween end might be cut off.")]
    private float responseModifier = 0.9f;

    [Header("---Prefab Refs---")]
    [SerializeField]
    private Transform playerModelHandle;

    //runtime data
    private Coroutine moveTween;

    public bool IsMoving { get => moveTween != null; }

    public void Move(Vector3 input)
    {   //prevent spamming and moving too fast
        if (IsMoving    //don't animate to zero
            || (input.x == 0 && input.y == 0))
            return;

        //calculate target destination for lerp
        var forwardMove = input.z * stepAmount.z;

        //prioritize forward movement over horizontal.
        var horizontalMove = (forwardMove == 0) 
            ? input.x * stepAmount.x : 0;

        var destination = new Vector3(horizontalMove, 0, forwardMove)
            + playerModelHandle.position;

        moveTween = StartCoroutine(Tween_Lerp(destination, moveAnimTime));
    }

    private IEnumerator Tween_Lerp(Vector3 targetPoint, 
        float duration)
    {
        var startTime = ApexGameController.UpTime;
        var endTime = startTime + duration;
        var runtime = 0.0f;

        yield return null; //skip current frame

        while (runtime < duration * responseModifier)
        {
            yield return null; //wait for next frame.
            
            runtime += ApexGameController.DeltaTime;//frame rate independent
            var percentComplete = runtime / duration;

            playerModelHandle.position = Vector3.Lerp(
                playerModelHandle.position, targetPoint, percentComplete);
        }
        playerModelHandle.position = targetPoint; //finish move and prevent overshoot
        moveTween = null;//flag anim complete
    }
}
