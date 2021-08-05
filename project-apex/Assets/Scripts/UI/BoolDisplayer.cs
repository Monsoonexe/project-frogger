using UnityEngine;
using TMPro;

public class BoolDisplayer : ApexMonoBehaviour
{
    [Header("---Settings---")]
    public string trueMessage = "On";
    public string falseMessage = "Off";

    [Header("---Resources---")]
    [SerializeField]
    private BoolVariable data;

    [Header("---Prefab Refs---")]
    [SerializeField]
    private TextMeshProUGUI textGUI;

    private void Reset()
    {
        SetDevDescription("I show a string based on a boolean value.");
    }
    private void OnEnable()
    {
        data.onValueChanged += UpdateUI;
        UpdateUI();
    }
    private void OnDisable()
    {
        data.onValueChanged -= UpdateUI;
    }

    public void UpdateUI()
    {
        //determine which message to print.
        var outString = string.Empty;
        if (data.Value == true)
            outString = trueMessage;
        else
            outString = falseMessage;
        textGUI.text = outString;
    }
}
