using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Hooks Input events to UnityEvents which can be easily rigged in Inspector.
/// </summary>
public class InputListener : ApexMonoBehaviour
{
    public enum ButtonInteractType
    {
        Down,
        Up,
        Hold
    }

    [Header("---Settings---")]
    [SerializeField]
    private bool useKeyCode = true;

    [SerializeField]
    private KeyCode keycode = KeyCode.Space;

    [SerializeField]
    private bool useButton = false;

    [SerializeField]
    private StringVariable button;

    [SerializeField]
    private ButtonInteractType buttonType = ButtonInteractType.Down;

    [SerializeField]
    private UnityEvent action = new UnityEvent();

    private void Reset()
    {
        SetDevDescription("Hooks Input events to UnityEvents, " +
            "which can be easily rigged in Inspector.");
    }


    protected void Update()
    {
        ReactToButtonPress();
    }

    [ContextMenu("Force()")]
    public void PerformAction()
    {
        action.Invoke();
    }

    protected void ReactToButtonPress()
    {
        //listen for button
        switch (buttonType)
        {
            case ButtonInteractType.Down:
                if (useKeyCode && Input.GetKeyDown(keycode)
                    || useButton && Input.GetButtonDown(button))
                    PerformAction();
                break;
            case ButtonInteractType.Up:
                if (useKeyCode && Input.GetKeyUp(keycode)
                    || useButton && Input.GetButtonUp(button))
                    PerformAction();
                break;
            case ButtonInteractType.Hold:
                if (useKeyCode && Input.GetKey(keycode)
                    || useButton && Input.GetButton(button))
                    PerformAction();
                break;
        }
    }
}
