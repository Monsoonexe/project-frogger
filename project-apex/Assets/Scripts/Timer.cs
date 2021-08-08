using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A pretty-accurate timer Mono.
/// Counts down and raises an event when timer hits 0. Can loop.
/// </summary>
public class Timer : ApexMonoBehaviour
{
    private bool paused = false;

    public bool Paused
    {
        get => paused;
        set
        {
            if (value == true)
                Stop();
            else
                Resume();
        }
    }

    [Header("---Settings---")]
    public bool startOnAwake = false;
    public bool loop = false;

    /// <summary>
    /// Total duration of timer. TOP
    /// </summary>
    [Tooltip("Total duration of timer. TOP")]
    public float TimerDuration = 0;

    /// <summary>
    /// Time remaining on current loop. COUNTDOWN
    /// </summary>
    [Tooltip("Time remaining on current loop. COUNTDOWN")]
    public float TimeRemaining = 0;

    /// <summary>
    /// Accumulated time that has elapsed since last Restart() or Init().
    /// Not guaranteed to be equal to TimerDuration at end of timer due to deltaTime.
    /// </summary>
    public float TimeEllapsed { get; private set; }

    /// <summary>
    /// 0 will effectively pause effect but won't pause coroutine.
    /// </summary>
    [Tooltip("0 will effectively pause effect but won't pause coroutine.")]
    public float timeScale = 1.0f;

    public float PercentComplete
    {
        get => (TimerDuration - TimeRemaining) / TimerDuration;
    }

    [Header("---Events---")]
    [SerializeField]
    private UnityEvent onTimerExpire = new UnityEvent();
    public UnityEvent OnTimerExpire { get => onTimerExpire; }

    //runtime data
    private Coroutine timerRoutine;

    private void Reset()
    {
        SetDevDescription("Counts down and raises an event when timer hits 0. Can loop.");
    }

    protected void Awake()
    {
        if (startOnAwake) Restart();
    }

    private void OnDisable()
    {
        Stop();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="duration">Length of timer.</param>
    /// <param name="callback">Override current callback with this one.</param>
    /// <param name="loop">Auto-repeat at end?</param>
    public void Initialize(float duration, Action callback, bool loop = false)
    {
        Debug.Assert(callback != null,
            "Callback is null and shouldn't be.", this);

        OnTimerExpire.RemoveAllListeners();
        OnTimerExpire.AddListener(callback.Invoke);//
        Initialize(duration, loop);//forward call

    }

    public void Initialize(float duration, bool loop = false)
    {
        TimerDuration = duration;
        this.loop = loop;
        paused = false;

        //prevent multiple routines
        if (timerRoutine != null)
            StopCoroutine(timerRoutine);
        timerRoutine = StartCoroutine(TickTimer());
    }

    public void Stop()
    {
        paused = true;
        if (timerRoutine != null)
            StopCoroutine(timerRoutine);
    } 

    public void Restart()
        => Initialize(TimerDuration, loop);
    
    public void Resume()
    {
        paused = false;

        //prevent multiple routines
        if (timerRoutine != null)
            StopCoroutine(timerRoutine);
        timerRoutine = StartCoroutine(TickTimer());
    }

    /// <summary>
    /// Will nearly immediately complete, just not right away.
    /// </summary>
    public void ForceCompleteEventually()
    {
        TimeRemaining = 0;
    }

    /// <summary>
    /// tick... tick... tick...
    /// </summary>
    /// <returns></returns>
    /// <remarks>TODO - Could greatly reduce Coroutine iteration by using WaitForSeconds
    /// and calculating the runtime values in the functions.</remarks>
    private IEnumerator TickTimer()
    {
        //why would this function generate ~50 Bytes of memory?
        do // loop timer
        {
            TimeRemaining = TimerDuration;//reset timer
            TimeEllapsed = 0;

            //countdown loop
            while (TimeRemaining > 0)
            {
                yield return null;//wait for next frame
                var scaledDeltaTime = ApexGameController.DeltaTime * timeScale;
                TimeEllapsed += scaledDeltaTime;//track total time (in case duration was modified while running)
                TimeRemaining -= scaledDeltaTime;//tick... tick... tick...
            }
            TimeRemaining = 0;//set to prevent overshoot
            onTimerExpire.Invoke(); //ring ring ring
        } while (loop);
    }

    #region Constructors

    /// <summary>
    /// Creates a new inactive Timer.
    /// </summary>
    /// <returns></returns>
    public static Timer Construct()
        => new GameObject(typeof(Timer).Name).AddComponent<Timer>();

    #endregion

    #region Testing
#if UNITY_EDITOR

    public void Test1SecondTimer()
    {

    }

#endif
    #endregion

}
