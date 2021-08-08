using System;
using System.Collections;
using UnityEngine;

public static class ApexTweens
{
    /// <summary>
    /// Holds the coroutines that back tweens so disabling components won't kill animation.
    /// </summary>
    private static TweenHolder tweenHolder;

    /// <summary>
    /// Helps to prevent items looking like not moving.
    /// </summary>
    public const float LERP_EASE_BLEND = 0.9f; //

    private static IEnumerator Tween_LerpRoutine(
        this Transform transform, Vector3 targetPoint,
        float duration, Action onComplete = null)
    {
        var startTime = ApexGameController.Time;
        var endTime = startTime + duration;
        var runtime = 0.0f;

        yield return null; //skip current frame
        var runDuration = duration * LERP_EASE_BLEND;

        while (runtime < runDuration)
        {
            yield return null; //wait for next frame.

            runtime += ApexGameController.DeltaTime;//frame rate independent
            var percentComplete = runtime / duration;

            transform.position = Vector3.Lerp(
                transform.position, targetPoint, percentComplete);
        }
        transform.position = targetPoint; //finish move and prevent overshoot
        onComplete?.Invoke();
    }

    public static Coroutine Tween_Lerp(this Transform transform, 
        Vector3 targetPoint,
        float duration, Action onComplete = null)
    {
        return tweenHolder.StartCoroutine(
            transform.Tween_LerpRoutine(targetPoint, duration, onComplete));
    }

    public static void Init()
    {
        var tweenObj = new GameObject("Apex Tween Utility");
        tweenHolder = tweenObj.AddComponent<TweenHolder>();
    }

    /// <summary>
    /// Delay the invokation of a method for a certain time.
    /// Due to the passing of a function (ref type), this causes a GC allocation of about 24 B.
    /// Quick and dirty. Consider inlining you own implementation.
    /// </summary>
    public static void InvokeAfterDelay(Action callback, float delay)
        => tweenHolder.StartCoroutine(InvokeAfterDelayRoutine(
            callback, delay));

    /// <summary>
    /// Delay the invokation of a method for a certain time.
    /// Due to the passing of a function (ref type), this causes a GC allocation of about 24 B.
    /// Quick and dirty. Consider inlining you own implementation.
    /// </summary>
    public static IEnumerator InvokeAfterDelayRoutine(
        Action callback, float delay)
    {
        var callTime = ApexGameController.Time + delay;

        //checking every frame has more overhead due to resuming
        //this function, but will gen less garbage without a new WaitForSeconds();
        do yield return null;
        while (ApexGameController.Time < callTime);

        callback();
    }
}
