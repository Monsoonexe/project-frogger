using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerVolume : ApexMonoBehaviour
{
    [SerializeField]
    [Tooltip("Empty means 'any'.")]
    protected string reactToTag = "Player";

    [SerializeField]
    protected UnityEvent enterEvent = new UnityEvent();

    [SerializeField]
    protected UnityEvent exitEvent = new UnityEvent();

    [ContextMenu("ForceCollide()")]
    public void ForceCollide()
    {
        enterEvent.Invoke();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (string.IsNullOrEmpty(reactToTag) 
            || col.gameObject.CompareTag(reactToTag))
        {
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

    protected void OnCollisionEnter(Collision collision)
    {
        if (string.IsNullOrEmpty(reactToTag)
            || collision.gameObject.CompareTag(reactToTag))
        {
            enterEvent.Invoke();
        }
    }

    protected void OnCollisionExit(Collision collision)
    {
        if (string.IsNullOrEmpty(reactToTag)
            || collision.gameObject.CompareTag(reactToTag))
        {
            exitEvent.Invoke();
        }
    }
}
