using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApexMobile : MonoBehaviour
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
