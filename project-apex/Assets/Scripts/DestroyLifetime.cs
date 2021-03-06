using UnityEngine;

/// <summary>
/// Destroys parent GameObject after given time.
/// </summary>
public class DestroyLifetime : ApexMonoBehaviour
{
    [Header("---Settings---")]
    [Min(0)]
    public float lifeTime = 10;

    private void Reset()
    {
        SetDevDescription("Destroys parent GameObject after given time.");
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
