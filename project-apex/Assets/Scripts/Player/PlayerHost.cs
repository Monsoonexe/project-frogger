using UnityEngine;

/// <summary>
/// Helps manage the Player.
/// </summary>
public class PlayerHost : ApexMonoBehaviour
{
    public static PlayerHost Instance { get; private set; }

    [Header("---Settings---")]
    [SerializeField]
    [Min(0)]
    [Tooltip("Time after respawn that player should be inactive.")]
    private float respawnResumeDelay = 1.5f;

    [SerializeField]
    [Min(0)]
    private float deathInterval = 0.5f;

    [Min(0)]
    public float deathReflectionInterval = 1.0f;

    [Header("---Resources---")]
    [SerializeField]
    private IntVariable playerDeathCounter;

    [Header("---Prefab Refs---")]
    [SerializeField]
    private Transform playerMobileHandle;

    [SerializeField]
    private PlayerInputController playerInput;

    [SerializeField]
    private Collider playerCollider;

    [SerializeField]
    private ParticleSystem playerHitEffect;

    [Header("---Scene Refs---")]
    [SerializeField]
    private Transform respawnPoint;

    [Header("---Audio---")]
    [SerializeField]
    private AudioClip onPlayerDie;

    //runtime data
    public bool IsDead { get; private set; }

    private void Reset()
    {
        SetDevDescription("Helps manage the Player.");
    }

    private void OnValidate()
    {
        if(deathInterval < deathReflectionInterval)
        {
            //swap
            var temp = deathInterval;
            deathInterval = deathReflectionInterval;
            deathReflectionInterval = temp;
        }
    }

    private void Awake()
    {
        Debug.Assert(playerCollider != null,
            "[" + GetType().Name + "] " +
            "collider is null and should be set by developer.");

        Debug.Assert(playerInput != null,
            "[" + GetType().Name + "] " +
            "player input is null and should be set by developer.");

        Debug.Assert(playerMobileHandle != null,
            "[" + GetType().Name + "] " +
            "playerMobileHandle is null and should be set by developer.");

        Debug.Assert(playerDeathCounter != null,
            "[" + GetType().Name + "] " +
            "death Rariable is null and should be set by developer.");

        //easy singleton
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Singleton Error!", this);
        }
    }

    public void KillPlayer()
    {
        //prevent spamming
        if (IsDead) return;

        IsDead = true;

        //TODO screen shake
        playerHitEffect.Play();

        onPlayerDie.Play();

        //disabled movement and such
        TogglePlayerEnabled(enabled: false);

        ++playerDeathCounter.Value;//updates ui value 

        //wait for a tad to let the loss sink in
        ApexTweens.InvokeAfterDelay(
            //screen animtion to block out teleort
            () => ScreenTransitionUI.TriggerRandomTransitionEvent(
                ScreenTransitionUI.ETransitionType.OUT),
            deathReflectionInterval); //wait for a tad to let the loss sink in

        //respawn player
        ApexTweens.InvokeAfterDelay(RespawnPlayer, deathInterval);
    }

    public void RespawnPlayer()
    {
        playerMobileHandle.position = respawnPoint.position;

        //remove screen animtion to block out teleort
        ScreenTransitionUI.TriggerRandomTransitionEvent(
                ScreenTransitionUI.ETransitionType.IN);

        //re-enable input after some time.
        ApexTweens.InvokeAfterDelay(
            ConfigurePlayerAlive, respawnResumeDelay);
    }

    public void ConfigurePlayerAlive()
    {
        IsDead = false;
        TogglePlayerEnabled(true);
    }

    /// <summary>
    /// En/Disables Input and Collision.
    /// </summary>
    /// <param name="enabled"></param>
    public void TogglePlayerEnabled(bool enabled)
    {
        playerInput.enabled = enabled;//cut player controls
        playerCollider.enabled = enabled; //stop receiving collisions
    }

    /// <summary>
    /// Flips enabled-ness.
    /// </summary>
    public void TogglePlayerEnabled()
        => TogglePlayerEnabled(!playerInput.enabled);
}
