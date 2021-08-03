using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class ATile : ApexMonoBehaviour
{
    public static ATile CurrentTile { get; protected set; }

    public virtual void OnEnterTile()
    {
        CurrentTile?.OnExitTile();
        CurrentTile = this;
    }

    public virtual void OnExitTile()
    {
        CurrentTile = null;
    }
}
