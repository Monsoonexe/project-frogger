using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Can give Start() behaviour to non-Mono behaviour.
/// </summary>
public class UnityCallbackMono : ApexMonoBehaviour
{
    [Header("---Events---")]
    public UnityEvent startEvent = new UnityEvent();

    private void Reset()
    {
        SetDevDescription("Can give Start() behaviour to non-Mono behaviour.");
    }

    // Start is called before the first frame update
    private void Start()
    {
        startEvent.Invoke();
    }
}
