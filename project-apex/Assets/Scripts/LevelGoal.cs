using System.Collections;
using UnityEngine;

/// <summary>
/// The level is won when the Player enters my volume.
/// </summary>
public class LevelGoal : ApexMonoBehaviour
{
    [Header("---Animation---")]
    [SerializeField]
    private float celebrationTimeInterval = 5.0f;

    [Header("---Scene Refs---")]
    [SerializeField]
    private LevelController levelController;

    [Header("---Resources---")]
    [SerializeField]
    private AudioClip reachGoalClip;

    [SerializeField]
    private ScriptableGameEvent onLevelComplete;

    //runtime values
    private Coroutine winRoutine;

    public bool IsAnimating { get => winRoutine != null; }

    private void Reset()
    {
        SetDevDescription("The level is won when the Player enters my volume." +
            "Anything else that should happen immediately upon victory can be " +
            "rigged to TriggerVolume event.");

        levelController = FindObjectOfType<LevelController>();
    }

    public void OnPlayerReachGoal()
    {
        //prevent spamming
        if (IsAnimating) return;

        winRoutine = StartCoroutine(WinRoutine());
    }

    private IEnumerator WinRoutine()
    {
        //TODO - celebrate!
        //sound
        reachGoalClip.Play();
        onLevelComplete.Raise();

        //particle
        //UI message

        //wait a bit to soak it in
        yield return new WaitForSeconds(celebrationTimeInterval);

        //move to next scene
        levelController.LoadNextLevel();

        winRoutine = null;//clear flag
    }
}
