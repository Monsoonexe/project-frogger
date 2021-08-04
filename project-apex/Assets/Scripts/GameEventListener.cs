using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : ApexMonoBehaviour
{
    [Header("---Resources---")]
    [SerializeField]
    private ScriptableGameEvent triggerEvent;

    [SerializeField]
    private UnityEvent response = new UnityEvent();

    private void Reset()
    {
        SetDevDescription("I am a flexible event rigger! " +
            "Rig virtually anything you want to me!");
    }

    private void OnEnable()
    {
        triggerEvent.Event.AddListener(response.Invoke);
    }

    private void OnDisable()
    {
        triggerEvent.Event.RemoveListener(response.Invoke);
    }
}
