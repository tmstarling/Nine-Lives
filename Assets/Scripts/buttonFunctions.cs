using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class buttonFunctions : MonoBehaviour
{
    public static buttonFunctions Instance;

    [Header("Cheat Input")]
    [SerializeField] TMP_InputField cheatInputField;

    [Header("Cheat Text")]
    [SerializeField] TMP_Text cheatFeedbackText;

    [Header("Cheat Toggles")]
    [SerializeField] Toggle godModeToggle;
    [SerializeField] Toggle speedBoostToggle;
    [SerializeField] Toggle invulnerabilityToggle;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // ONLY ONE
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        godModeToggle.onValueChanged.AddListener(OnGodModeToggle);
        speedBoostToggle.onValueChanged.AddListener(OnSpeedBoostToggle);
        invulnerabilityToggle.onValueChanged.AddListener(OnInvulnerabilityToggle);

        SyncCheatToggles();
    }


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
            //Cheats Reapplied
            CheatManager.Instance.ApplyCheatsToPlayer(gamemanager.instance.playerScript);
            SyncCheatToggles();
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

        CheatManager.Instance.ActivateCheat(cheatCode);
        StartCoroutine(cheatcodeFeedback());

        switch (cheatCode)
        {
            case "godmode":
                cheatFeedbackText.text = "God mode activated";
                break;
            case "speedboost":
                cheatFeedbackText.text = "Speed boost activated";
                break;
            case "invulnerable":
                cheatFeedbackText.text = "Invulnerability activated";
                break;
            default:
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

    void OnGodModeToggle(bool isOn)
    {
        CheatManager.Instance.godModeEnabled = isOn;
        CheatManager.Instance.ApplyCheatsToPlayer(gamemanager.instance.playerScript);
    }

    void OnSpeedBoostToggle(bool isOn)
    {
        CheatManager.Instance.speedBoostEnabled = isOn;
        CheatManager.Instance.ApplyCheatsToPlayer(gamemanager.instance.playerScript);
    }

    void OnInvulnerabilityToggle(bool isOn)
    {
        CheatManager.Instance.invulnerabilityEnabled = isOn;
        CheatManager.Instance.ApplyCheatsToPlayer(gamemanager.instance.playerScript);
    }

    void SyncCheatToggles()
    {
        godModeToggle.isOn = CheatManager.Instance.godModeEnabled;
        speedBoostToggle.isOn = CheatManager.Instance.speedBoostEnabled;
        invulnerabilityToggle.isOn = CheatManager.Instance.invulnerabilityEnabled;
    }

}




