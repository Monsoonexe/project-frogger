using System;
using UnityEngine;

//clarifications
using Random = UnityEngine.Random;

/// <summary>
/// Handles cool fade to/from black screen transitions.
/// Uses static events instead of singleton pattern.
/// </summary>
public class ScreenTransitionUI : ApexMonoBehaviour
{
    public enum ETransitionType
    {
        /// <summary>
        /// Black overtakes to block scene.
        /// </summary>
        IN,

        /// <summary>
        /// Black goes away to reveal scene.
        /// </summary>
        OUT
    }
    public static Action<ETransitionType> TriggerRandomTransitionEvent;

    [Header("---Scene Refs---")]
    [SerializeField]
    private Animator myAnimator;

    [Header("---Animator Messages---")]
    [SerializeField]
    private string animateInMessage = "In";

    [SerializeField]
    private string animateOut_X_Message = "Out_X";
    public string AnimateOut_X_Message { get => animateOut_X_Message; } //readonly

    [SerializeField]
    private string animateOut_Y_Message = "Out_Y";
    public string AnimateOut_Y_Message { get => animateOut_Y_Message; } //readonly

    private void Reset()
    {
        SetDevDescription("Handles cool fade to/from black screen transitions.");
    }

    private void OnEnable()
    {
        TriggerRandomTransitionEvent += TriggerRandomTransitionHandler;
    }

    private void OnDisable()
    {
        TriggerRandomTransitionEvent -= TriggerRandomTransitionHandler;
    }

    private void TriggerRandomTransitionHandler(ETransitionType transition)
    {
        string animatorMessage = null;

        //determine desired
        switch (transition)
        {
            case ETransitionType.IN:
                animatorMessage = animateInMessage;
                break;
            case ETransitionType.OUT:
                animatorMessage = (Random.Range(0.0f, 1.0f) <= 0.5f)//coin flip
                    ? animateOut_X_Message : animateOut_Y_Message;
                break;
            default:
                Debug.LogWarning("Case not accounted for: " + transform.ToString());
                break;
        }

        //validate transition
        if(!string.IsNullOrEmpty(animatorMessage))
            myAnimator.SetTrigger(animatorMessage);
    }
}
