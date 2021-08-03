using UnityEngine;

public static class GameObject_Extensions
{
    public static bool TryGetComponent<TComponent>(
        this GameObject gameObject, out TComponent component)
        where TComponent : Component
    {
        component = gameObject.GetComponent<TComponent>();
        return component != null;
    }
}
