using UnityEngine;

public class AudioManager : MonoBehaviour
{

    private static readonly string AUDIO_MANAGER_NAME = "AudioManager";
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (!_instance)
                CreateInstance();
            return _instance;
        }
        private set => _instance = value;
    }

    [SerializeField]
    private AudioSource[] sources;

    private void Awake()
    {
        //init singleton
        if (!_instance)
            _instance = this;
        else
            Debug.LogWarning("Singleton Error!", this);
    }

    public void Play(AudioClip clip)
    {
        if (clip == null) return; // early exit

        //play from first idle source
        var len = sources.Length;
        for (var i = 0; i < len; ++i)
            if (!sources[i].isPlaying)
            {
                sources[i].PlayOneShot(clip);
                Debug.Log("playing from source: " + i);
                break;
            }
    }

    private static void CreateInstance()
    {
        var gameObject = Resources.Load(AUDIO_MANAGER_NAME) as GameObject;
        if (gameObject)
        {
            var _instance = gameObject.GetComponent<AudioManager>();
            if (_instance == null)
            {
                //TODO - complain
            }
        }
        else
        {
            //TODO - complain
        }
    }
}

public static class AudioManager_Extensions
{
    /// <summary>
    /// Play audio clip from anywhere.
    /// </summary>
    /// <param name="clip"></param>
    public static void Play(this AudioClip clip)
        => AudioManager.Instance.Play(clip);
}
