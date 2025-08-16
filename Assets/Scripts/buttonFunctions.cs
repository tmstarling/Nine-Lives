using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class buttonFunctions : MonoBehaviour
{
    [SerializeField] TMP_InputField cheatInputField;
    [SerializeField] TMP_Text cheatFeedbackText;

    public void resume()
    {
        gamemanager.instance.stateUnpaused();
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gamemanager.instance.stateUnpaused();
    }

    public void cheats()
    {
        gamemanager.instance.youCheat();
    }

    public void respawn()
    {
        if (CheckpointManager.instance == null) 
        {
            Debug.LogError("CheckpointManager.instance is null");
        }
        else
        {
            CheckpointManager.instance.ResetToLastCheckpoint();
            gamemanager.instance.stateUnpaused();
        }
    }

    public void quit()
    {
    #if !UNITY_EDITOR
        Application.Quit();
    #else
    
        
       UnityEditor.EditorApplication.isPlaying = false;    

    #endif

    }

    public void ClearCheat()
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
                player.EnableSpeedBoost();
                player.EnableInvulnerability();

                StartCoroutine(cheatcodeFeedback());

                cheatFeedbackText.text = "God mode activated";
                break;

            case "speedboost":
                player.EnableSpeedBoost();

                StartCoroutine(cheatcodeFeedback());

                cheatFeedbackText.text = "Speed boost activated";
                break;

             //case "wallhack":
             //   player.EnableWallHack();

             //   StartCoroutine(cheatcodeFeedback());

             //   cheatFeedbackText.text = "Wallhack activated";
             //   break;

            case "invulnerable":
                player.EnableInvulnerability();

                StartCoroutine(cheatcodeFeedback());

                cheatFeedbackText.text = "Invulnerability activated";
                break;

            //case "spawnenemy":
            //    player.SpawnEnemy();

            //    StartCoroutine(cheatcodeFeedback());

            //    cheatFeedbackText.text = "Item drop activated";
            //    break;

            case "spawnitem":
                if (ItemSpawner.instance != null)
                {
                    ItemSpawner.instance.SpawnItem();

                    StartCoroutine(cheatcodeFeedback());

                    cheatFeedbackText.text = "Item spawned via cheat";
                }
                else
                {
                    Debug.LogWarning("ItemSpawner instance not found");
                }
                break;

            default:
                StartCoroutine(cheatcodeFeedback());

                cheatFeedbackText.text = "Unknown cheat code: " + cheatCode;
                break;
        }

        cheatInputField.text = "";
    }
    IEnumerator cheatcodeFeedback()
    {
        gamemanager.instance.cheatPopup.SetActive(true);
        yield return new WaitForSeconds(1);
        gamemanager.instance.cheatPopup.SetActive(false);
    }
}




