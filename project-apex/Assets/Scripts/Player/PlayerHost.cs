using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helps manage the Player.
/// </summary>
public class PlayerHost : ApexMonoBehaviour
{
    public static PlayerHost Instance { get; private set; }

    [Header("---Settings---")]
    [SerializeField]
    [Tooltip("Time after respawn that player should be inactive.")]
    private float respawnResumeDelay = 1.5f;

    [SerializeField]
    private float deathInterval = 0.5f;

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

    [Header("---Scene Refs---")]
    [SerializeField]
    private Transform respawnPoint;

    private void Reset()
    {
        SetDevDescription("Helps manage the Player.");
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
        playerInput.enabled = false;//cut player controls
        //TODO immediately play SFX and screen shake
        ++playerDeathCounter.Value;

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
            () => playerInput.enabled = true, respawnResumeDelay);
    }
}
