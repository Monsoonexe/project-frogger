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

    [Header("---Resources---")]
    [SerializeField]
    private IntVariable playerDeathCounter;

    [Header("---Prefab Refs---")]
    [SerializeField]
    private Transform playerMobileHandle;

    [SerializeField]
    private PlayerInputController playerInput;

    [Header("---Scene Refs---")]
    [SerializeField]
    private Transform respawnPoint;

    private void Reset()
    {
        SetDevDescription("Helps manage the Player.");
    }

    private void Awake()
    {
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
        ApexTweens.InvokeAfterDelay(RespawnPlayer, deathInterval);
    }

    public void RespawnPlayer()
    {
        playerMobileHandle.position = respawnPoint.position;

        //re-enable input after some time.
        ApexTweens.InvokeAfterDelay(
            () => playerInput.enabled = true, respawnResumeDelay);
    }
}
