using System;
using UnityEngine;

/// <summary>
/// Handles cool fade to/from black screen transitions.
/// Uses static events instead of singleton pattern.
/// </summary>
public class ScreenTransitionUI : ApexMonoBehaviour
{
    public static Action<string> TriggerTransitionEvent;

    [Header("---Scene Refs---")]
    [SerializeField]
    private Animator myAnimator;

    private void Reset()
    {
        SetDevDescription("Handles cool fade to/from black screen transitions.");
    }

    private void OnEnable()
    {
        TriggerTransitionEvent += TriggerTransition;
    }

    private void OnDisable()
    {
        TriggerTransitionEvent -= TriggerTransition;
    }

    private void TriggerTransition(string transitionName)
    {
        //TODO input validation and constants in custom editors
        myAnimator.SetTrigger(transitionName);
    }
}
