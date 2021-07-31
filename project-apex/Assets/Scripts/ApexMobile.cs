using UnityEngine;

public abstract class ApexMobile : ApexMonoBehaviour
{
    /// <summary>
    /// Cache because '.transform' is secretly GetComponent().
    /// </summary>
    public Transform myTransform { get; set; }

    protected virtual void Awake()
    {
        myTransform = GetComponent<Transform>();
    }
}
