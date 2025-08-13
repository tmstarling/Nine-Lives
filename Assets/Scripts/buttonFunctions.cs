using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class buttonFunctions : MonoBehaviour
{
    [SerializeField] AudioSource hoverSFX;
    [SerializeField] AudioSource clickSFX;

    [SerializeField] TMP_InputField cheatInputField;


    public void resume()
    {
        gamemanager.instance.stateUnpaused();
    }

    public void restart()
    {
        CheckpointManager.instance.ResetToLastCheckpoint();
        gamemanager.instance.stateUnpaused();
    }

    public void cheats()
    {
        Debug.Log("Cheat button clicked");
        gamemanager.instance.youCheat();
    }

    // ... existing methods ...

    public void ClearCheatInput()
    {
        if (cheatInputField != null)
        {
            cheatInputField.text = "";
        }
    }

    public void EnterCheat()
    {
        string cheatCode = cheatInputField.text.Trim().ToLower();
        Debug.Log("Entered cheat: " + cheatCode);

        var player = gamemanager.instance.playerScript;

        switch (cheatCode)
        {
            case "godmode":
                player.godMode = true;
                Debug.Log("God mode activated");
                break;

            case "speedboost":
                player.EnableSpeedBoost();
                Debug.Log("Speed boost activated");
                break;

            case "wallhack":
                player.EnableWallHack();
                Debug.Log("Wallhack activated");
                break;

            case "invulnerable":
                player.EnableInvulnerability();
                Debug.Log("Invulnerability activated");
                break;

            //case "spawn enemy":
            //    player.SpawnEnemy();
            //    Debug.Log("Item drop activated");
            //    break;

            default:
                Debug.LogWarning("Unknown cheat code: " + cheatCode);
                break;
        }

        cheatInputField.text = "";
    }


    public void quit()
    {
    #if !UNITY_EDITOR
        Application.Quit();
    #else
    
        
       UnityEditor.EditorApplication.isPlaying = false;    

    #endif

    }

    public void hover()
    {
        hoverSFX.Play();
    }
    public void click()
    {
        clickSFX.Play();
    }
}