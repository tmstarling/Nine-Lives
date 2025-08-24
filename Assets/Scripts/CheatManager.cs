using UnityEngine;
using System.Collections;

public class CheatManager : MonoBehaviour
{
    public static CheatManager Instance;

    [Header("Cheat flags")]
    public bool flyModeEnabled;
    public bool speedBoostEnabled;
    public bool invulnerabilityEnabled;

    private void Awake()
    {
        transform.parent = null;
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
        if (flyModeEnabled)
        {
            gamemanager.instance.playerScript.flyMode = true;
        }
        else
        {
            gamemanager.instance.playerScript.flyMode = false;
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
            case "flymode":
                if (flyModeEnabled) return;

                flyModeEnabled = true;
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
        flyModeEnabled = false;
        speedBoostEnabled = false;
        invulnerabilityEnabled = false;

        ApplyCheatsToPlayer();
    }

    public void EnableFlyMode()
    {
        gamemanager.instance.playerScript.flyMode = true;
    }

    public void DisableGodMode()
    {
        gamemanager.instance.playerScript.flyMode = false;
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
