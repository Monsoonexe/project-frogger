using UnityEngine;
using TMPro;

/// <summary>
/// Observes a Variable and displays its value on events.
/// </summary>
public class VariableDisplayer : ApexMonoBehaviour
{
    [Header("---Resources---")]
    [SerializeField]
    protected AVariable targetData;

    [Header("---Scene Refs---")]
    [SerializeField]
    protected TextMeshProUGUI textElement;

    protected virtual void Reset()
    {
        SetDevDescription("Observes a Variable and displays its value on events.");
    }

    protected virtual void OnEnable()
    {
        SubscribeToEvents();
        UpdateUI();
    }

    protected virtual void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    protected virtual void SubscribeToEvents()
    {
        targetData.onValueChanged += UpdateUI;
    }

    protected virtual void UnsubscribeFromEvents()
    {
        targetData.onValueChanged -= UpdateUI;
    }

    [ContextMenu("UpdateUI()")]
    public virtual void UpdateUI()
    {
        textElement.text = targetData.ToString();
    }
}
