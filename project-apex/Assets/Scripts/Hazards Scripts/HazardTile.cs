
/// <summary>
/// Kills player when entered.
/// </summary>
public class HazardTile : ATile
{
    private void Reset()
    {
        SetDevDescription("Kills Player on enter.");
    }

    public override void OnEnterTile()
    {
        base.OnEnterTile();
        PlayerHost.Instance.KillPlayer();
        OnExitTile();//cuz player is now dead.
    }
}
