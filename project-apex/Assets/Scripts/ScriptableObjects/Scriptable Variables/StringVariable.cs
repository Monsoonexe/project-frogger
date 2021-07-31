using UnityEngine;

/// <summary>
/// Static, project-level game data. These are great because typos suck!
/// </summary>
[CreateAssetMenu(
    fileName = "S_",
    menuName = CREATE_ASSET_MENU_PATH + "string")]
public class StringVariable : AVariable<string>
{
    //exists

    /// <summary>
    /// Returns base value.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
        => _value;
}
