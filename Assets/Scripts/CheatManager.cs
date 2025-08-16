using UnityEngine;

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

    public void ApplyCheatsToPlayer(PlayerController player)
    {
        if (player == null)
        {
            Debug.LogWarning("CheatManager: Player reference is null");
            return;
        }

        if (godModeEnabled)
        {
            player.godMode = true;
            player.EnableSpeedBoost();
            player.EnableInvulnerability();
        }

        if (speedBoostEnabled)
        {
            player.EnableSpeedBoost();
        }

        if (invulnerabilityEnabled)
        {
            player.EnableInvulnerability();
        }
    }

    public void ActivateCheat(string cheatCode)
    {
        var player = gamemanager.instance.playerScript;

        switch (cheatCode.ToLower().Trim())
        {
            case "godmode":
                godModeEnabled = true;
                speedBoostEnabled = true;
                invulnerabilityEnabled = true;
                ApplyCheatsToPlayer(player);
                break;

            case "speedboost":
                speedBoostEnabled = true;
                ApplyCheatsToPlayer(player);
                break;

            case "invulnerable":
                invulnerabilityEnabled = true;
                ApplyCheatsToPlayer(player);
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
    }
}
