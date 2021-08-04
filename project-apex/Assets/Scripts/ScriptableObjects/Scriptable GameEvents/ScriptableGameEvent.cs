using UnityEngine;
using UnityEngine.Events;

/// <seealso cref="GameEventListener"/>
[CreateAssetMenu(
    fileName = "_GameEvent",
    menuName = "ScriptableObjects/Game Event")]
public class ScriptableGameEvent : ApexScriptableObject
{
    public readonly UnityEvent Event = new UnityEvent();

    [ContextMenu("ForceRaise()")]
    public void Raise() => Event.Invoke();
}
