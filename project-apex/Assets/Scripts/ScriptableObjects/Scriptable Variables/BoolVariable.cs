using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <seealso cref="IntVariable"/>
[CreateAssetMenu(
    fileName = "B_",
    menuName = CREATE_ASSET_MENU_PATH + "bool")]
public class BoolVariable : AVariable<bool>
{
    public bool InvertedValue { get => !_value; }
    public void Invert() => Value = !_value;

    public override string ToString()
        => _value ? bool.TrueString : bool.FalseString;
}
