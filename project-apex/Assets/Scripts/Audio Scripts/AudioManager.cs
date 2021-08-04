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

    public void Play(AudioClip clip)
    {
        if (clip == null) return; // early exit

        //play from first idle source
        var len = sources.Length;
        for (var i = 0; i < len; ++i)
            if (!sources[i].isPlaying)
            {
                var source = sources[i];
                source.clip = clip;
                source.Play();
                //Debug.Log("playing from source: " + i);
                break;
            }
    }

    public static void Init()
    {
        var inSceneInstance = FindObjectOfType<AudioManager>();
        if (!inSceneInstance)
            CreateInstance();
    }

    private static void CreateInstance()
    {
        var prefab = Resources.Load(AUDIO_MANAGER_NAME) as GameObject;

        if (prefab)
        {
            var objInstance = Instantiate(prefab);
            _instance = objInstance.GetComponent<AudioManager>();
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
