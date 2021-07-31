﻿using System;
using System.Collections;
using UnityEngine;

public static class ApexTweens
{
    /// <summary>
    /// Holds the coroutines that back tweens so disabling components won't kill animation.
    /// </summary>
    private static TweenHolder tweenHolder;
    public static bool IsInitialized { get; private set; }

    /// <summary>
    /// Helps to prevent items looking like not moving.
    /// </summary>
    public const float LERP_EASE_BLEND = 0.9f; //

    private static IEnumerator Tween_LerpRoutine(
        this Transform transform, Vector3 targetPoint,
        float duration, Action onComplete = null)
    {
        var startTime = ApexGameController.UpTime;
        var endTime = startTime + duration;
        var runtime = 0.0f;

        yield return null; //skip current frame

        while (runtime < duration * LERP_EASE_BLEND)
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

    public static Coroutine Tween_Lerp(this Transform transform, Vector3 targetPoint,
        float duration, Action onComplete = null)
    {
        return tweenHolder.StartCoroutine(
            transform.Tween_LerpRoutine(targetPoint, duration, onComplete));
    }

    public static void Init()
    {
        if (IsInitialized) return;

        var tweenObj = new GameObject("Apex Tween Utility");
        tweenHolder = tweenObj.AddComponent<TweenHolder>();
        UnityEngine.Object.DontDestroyOnLoad(tweenObj);
    }

    /// <summary>
    /// Delay the invokation of a method for a certain time.
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="delay"></param>
    public static void InvokeAfterDelay(Action callback, float delay)
        => tweenHolder.StartCoroutine(InvokeAfterDelayRoutine(
            callback, delay));

    private static IEnumerator InvokeAfterDelayRoutine(Action callback, float delay)
    {
        var callTime = ApexGameController.UpTime + delay;

        do yield return null;
        while (ApexGameController.UpTime < callTime);

        callback();
    }
}