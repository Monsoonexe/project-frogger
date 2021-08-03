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

    [Header("---Resources---")]
    [SerializeField]
    private IntVariable gameWinsCount;

    /// <summary>
    /// Time.deltaTime that has been cached and un-marshalled.
    /// </summary>
    public static float DeltaTime { get; private set; }

    /// <summary>
    /// Time.time that has been cached and un-marshalled.
    /// </summary>
    public static float UpTime { get; private set; }
    
    public static float FixedDeltaTime { get; private set; }

    private void Awake()
    {
        //initialize systems.
        ApexTweens.Init();
        AudioManager.Init();
    }

    private void Start()
    {
        Application.targetFrameRate = targetFrameRate.Value;

        //load data
        if (PlayerPrefs.HasKey(gameWinsCount.name))
            gameWinsCount.Value = PlayerPrefs.GetInt(gameWinsCount.name);
    }

    private void Update()
    {
        //cache time values to de-marshal them once
        DeltaTime = Time.deltaTime;
        UpTime = Time.time;

        //handle app-level input
        if (Input.GetButtonDown(quitButtonName))
            QuitGame();
        else if (Input.GetButtonDown(resetButtonName))
            ResetGame();
    }

    private void FixedUpdate()
    {
        //cache time values to de-marshal them once
        FixedDeltaTime = Time.fixedDeltaTime;
    }

    private void QuitGame()
    {
        PlayerPrefs.SetInt(gameWinsCount.name, gameWinsCount.Value);
        Application.Quit();
    }

    private void ResetGame()
    {
        SceneManager.LoadScene(0);
    }

    [ContextMenu("Delete PlayerPrefs Keys")]
    public void ClearSaveFile() => PlayerPrefs.DeleteAll();
}
