using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <seealso cref="IntVariable"/>
[CreateAssetMenu()]
public class BoolVariable : AVariable<bool>
{
    public bool InvertedValue { get => !_value; }
    public void Invert() => _value = !_value;

    public override string ToString()
        => _value ? bool.TrueString : bool.FalseString;
}
