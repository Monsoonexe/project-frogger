using UnityEngine;

/// <summary>
/// Static, project-level game data.
/// </summary>
/// <seealso cref="StringVariable"/>
[CreateAssetMenu(
    fileName = "I_",
    menuName = CREATE_ASSET_MENU_PATH + "int")]
public class IntVariable : AVariable<int>
{
    public void Add(int amount)
        => Value += amount;

    public void Subtract(int amount)
        => Value -= amount;

    /// <summary>
    /// Returns a likely-cached string.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
        => ConstStrings.GetCachedString(_value);
}
