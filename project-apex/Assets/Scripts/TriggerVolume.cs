using UnityEngine;
using UnityEngine.Events;

public class TriggerVolume : ApexMonoBehaviour
{
    [SerializeField]
    [Tooltip("Empty means 'any'.")]
    protected string reactToTag = "Player";

    /// <summary>
    /// Running total of times this has been triggered.
    /// </summary>
    [Tooltip("Running total of times this has been triggered.")]
    public int triggerCount = 0;

    [SerializeField]
    protected UnityEvent enterEvent = new UnityEvent();

    [SerializeField]
    protected UnityEvent exitEvent = new UnityEvent();

    [ContextMenu("ForceCollide()")]
    public void ForceCollide()
    {
        ++triggerCount;
        enterEvent.Invoke();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (string.IsNullOrEmpty(reactToTag) 
            || col.gameObject.CompareTag(reactToTag))
        {
            ++triggerCount;
            enterEvent.Invoke();
        }
    }

    protected void OnTriggerExit(Collider col)
    {
        if (string.IsNullOrEmpty(reactToTag) 
            || col.gameObject.CompareTag(reactToTag))
        {
            exitEvent.Invoke();
        }
    }
}
