using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the happenings for a particular level.
/// </summary>
public class LevelController : ApexMonoBehaviour
{
    [Header("---Settings---")]
    [SerializeField]
    private int nextLevelIndex = 1;

    [SerializeField]
    private float transitionDelay = 1;

    [Header("---Resources---")]
    [SerializeField]
    private IntVariable levelTracker;

    private void Start()
    {
        levelTracker.Value = SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadLevel(int buildIndex)
    {
        ScreenTransitionUI.TriggerRandomTransitionEvent(
            ScreenTransitionUI.ETransitionType.OUT);

        //load when screen is black.
        ApexTweens.InvokeAfterDelay(
            () => SceneManager.LoadScene(buildIndex),
            transitionDelay);
    }

    public void LoadNextLevel()
        => LoadLevel(nextLevelIndex);

    public void LoadNextLevelImmediately()
    {
        SceneManager.LoadScene(nextLevelIndex);
    }

    public void LoadFirstLevel() => LoadLevel(1);

    public void LoadMainMenu() => LoadLevel(0);
}
