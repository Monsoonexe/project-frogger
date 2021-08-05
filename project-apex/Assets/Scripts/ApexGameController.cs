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

    public bool clearSaveFileOnAwake = true;

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
    public static float Time { get; private set; }
    
    public static float FixedDeltaTime { get; private set; }

    private void Awake()
    {
        //initialize systems.
        ApexTweens.Init();
        AudioManager.Init();

        if (clearSaveFileOnAwake)
            ClearSaveFile();

        gameWinsCount.onValueChanged += SaveComplettions;
    }

    private void OnDestroy()
    {
        gameWinsCount.onValueChanged -= SaveComplettions;
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
        DeltaTime = UnityEngine.Time.deltaTime;
        Time = UnityEngine.Time.time;

        //handle app-level input
        if (Input.GetButtonDown(quitButtonName))
            QuitGame();
        else if (Input.GetButtonDown(resetButtonName))
            ResetGame();
    }

    private void FixedUpdate()
    {
        //cache time values to de-marshal them once
        FixedDeltaTime = UnityEngine.Time.fixedDeltaTime;
    }

    private void OnApplicationQuit()
    {
        SaveComplettions();
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void ResetGame()
    {
        SceneManager.LoadScene(0);
    }

    private void SaveComplettions()
    {
        PlayerPrefs.SetInt(gameWinsCount.name, gameWinsCount.Value);
    }

    [ContextMenu("Delete PlayerPrefs Keys")]
    public void ClearSaveFile() => PlayerPrefs.DeleteAll();
}
