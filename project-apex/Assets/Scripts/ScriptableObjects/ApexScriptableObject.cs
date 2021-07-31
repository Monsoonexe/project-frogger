using System.Diagnostics;
using UnityEngine;

/// <summary>
/// Base object for Scriptables in project Apex.
/// </summary>
public class ApexScriptableObject : ScriptableObject
{
#if UNITY_EDITOR
#pragma warning disable 0414 //disable "value is never used" warning
    [SerializeField]
    [TextArea]
    private string developerDescription = "Please enter a description or a note.";
#pragma warning restore
#endif

    /// <summary>
    /// This call will be stripped out of Builds. Use anywhere.
    /// </summary>
    /// <param name="newDes"></param>
    [Conditional("UNITY_EDITOR")]
    public void SetDevDescription(string newDes)
    {
#if UNITY_EDITOR
        developerDescription = newDes;
#endif
    }
}
