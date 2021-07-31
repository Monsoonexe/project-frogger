using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls settings and app-level stuff.
/// Has a lower execution-order than 'default'.
/// </summary>
public class ApexGameController : MonoBehaviour
{
    [Header("---Settings---")]
    [SerializeField]
    private IntVariable targetFrameRate;

    [Header("---Inputs---")]
    [SerializeField]
    private StringVariable quitButtonName;

    [SerializeField]
    private StringVariable resetButtonName;

    /// <summary>
    /// Time.deltaTime that has been cached and un-marshalled.
    /// </summary>
    public static float DeltaTime { get; private set; }

    private void Start()
    {
        Application.targetFrameRate = targetFrameRate;
    }

    private void Update()
    {
        DeltaTime = Time.deltaTime;//cache

        //handle input
        if (Input.GetButtonDown(quitButtonName))
            QuitGame();
        else if (Input.GetButtonDown(resetButtonName))
            ResetGame();
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void ResetGame()
    {
        SceneManager.LoadScene(0);
    }
}
