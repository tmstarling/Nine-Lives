using UnityEngine;
using System.Collections;

public class CheatManager : MonoBehaviour
{
    public static CheatManager Instance;

    [Header("Cheat flags")]
    public bool godModeEnabled;
    public bool speedBoostEnabled;
    public bool invulnerabilityEnabled;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ApplyCheatsToPlayer()
    {
        if (godModeEnabled)
        {
            gamemanager.instance.playerScript.godMode = true;
            EnableSpeedBoost();
            EnableInvulnerability();
        }
        else
        {
            gamemanager.instance.playerScript.godMode = false;
            DisableSpeedBoost();
            DisableInvulnerability();
        }

        if (speedBoostEnabled)
        {
            EnableSpeedBoost();
        }
        else
        {
            DisableSpeedBoost();
        }

        if (invulnerabilityEnabled)
        {
            EnableInvulnerability();
        }
        else
        {
            DisableInvulnerability();
        }
    }

    public void ActivateCheat(string cheatCode)
    {
        var player = gamemanager.instance.playerScript;

        switch (cheatCode.ToLower().Trim())
        {
            case "godmode":
                if (godModeEnabled) return;

                godModeEnabled = true;
                speedBoostEnabled = true;
                invulnerabilityEnabled = true;
                ApplyCheatsToPlayer();
                break;

            case "speedboost":
                if (speedBoostEnabled) return;

                speedBoostEnabled = true;
                ApplyCheatsToPlayer();
                break;

            case "invulnerable":
                if (invulnerabilityEnabled) return;

                invulnerabilityEnabled = true;
                ApplyCheatsToPlayer();
                break;

            default:
                Debug.Log("CheatManager: Unknown cheat code - " + cheatCode);
                break;
        }
    }

    public void ClearCheats()
    {
        godModeEnabled = false;
        speedBoostEnabled = false;
        invulnerabilityEnabled = false;

        ApplyCheatsToPlayer();
    }

    public void EnableGodMode()
    {
        gamemanager.instance.playerScript.godMode = true;
    }

    public void DisableGodMode()
    {
        gamemanager.instance.playerScript.godMode = false;
    }

    public void EnableSpeedBoost()
    {
        gamemanager.instance.playerScript.speed = gamemanager.instance.playerScript.boostedSpeed;
    }
    public void DisableSpeedBoost()
    {
        gamemanager.instance.playerScript.speed = gamemanager.instance.playerScript.originalSpeed;
    }

    public void EnableInvulnerability()
    {
        gamemanager.instance.playerScript.invulnerability = true;
    }
    public void DisableInvulnerability()
    {
        gamemanager.instance.playerScript.invulnerability = false;
    }

    public void ReapplyCheatsAfterRespawn()
    {
        if (gamemanager.instance.playerScript != null)
        {
            ApplyCheatsToPlayer();
            buttonFunctions.Instance.SyncCheatToggles();
        }
        else
        {
            Debug.LogWarning("PlayerScript not found after respawn");
        }
    }
}
