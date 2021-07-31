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

    [Header("---Resources---")]
    [SerializeField]
    private IntVariable levelTracker;

    private void Start()
    {
        levelTracker.Value = SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevelIndex);
    }
}
