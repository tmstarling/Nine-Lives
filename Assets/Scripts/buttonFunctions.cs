using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class buttonFunctions : MonoBehaviour
{
    [SerializeField] Renderer model;

    [SerializeField] AudioSource hoverSFX;
    [SerializeField] AudioSource clickSFX;

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
        Debug.Log("Cheat button clicked");
        gamemanager.instance.youCheat();
    }

    public void repawn()
    {
        CheckpointManager.instance.ResetToLastCheckpoint();
        gamemanager.instance.stateUnpaused();
    }

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

            case "invulnerable":
                player.EnableInvulnerability();

                StartCoroutine(cheatcodeFeedback());

                cheatFeedbackText.text = "Invulnerability activated";
                break;

            //case "spawnenemy":
            //    player.SpawnEnemy();

            //    StartCoroutine(cheatcodeFeedback());

            //    Debug.Log("Item drop activated");
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