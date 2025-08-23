using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class buttonFunctions : MonoBehaviour
{
    AudioSource hoverSFX;
    AudioSource clickSFX;

    public void hover()
    {
        hoverSFX.Play();
    }
    public void click()
    {
        clickSFX.Play();
    }
	
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
        clickSFX = SoundManager.Instance.click;
        hoverSFX = SoundManager.Instance.hover;

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

    public void audioMix()
    {
        gamemanager.instance.audioMixer();
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
            CheatManager.Instance.ReapplyCheatsAfterRespawn();
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
        SyncCheatToggles();
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
        if (!isOn)
        {
            CheatManager.Instance.godModeEnabled = false;
            CheatManager.Instance.ApplyCheatsToPlayer();
        }
        else
        {
            godModeToggle.isOn = false;
        }
    }

    void OnSpeedBoostToggle(bool isOn)
    {
        if (!isOn)
        {
            CheatManager.Instance.speedBoostEnabled = false;
            CheatManager.Instance.ApplyCheatsToPlayer();
        }
        else
        {
            speedBoostToggle.isOn = false;
        }
    }

    void OnInvulnerabilityToggle(bool isOn)
    {
        if (!isOn)
        {
            CheatManager.Instance.invulnerabilityEnabled = false;
            CheatManager.Instance.ApplyCheatsToPlayer();
        }
        else
        {
            invulnerabilityToggle.isOn = false;
        }
    }

    public void SyncCheatToggles()
    {
        //Temp Remove Listeners
        godModeToggle.onValueChanged.RemoveAllListeners();
        speedBoostToggle.onValueChanged.RemoveAllListeners();
        invulnerabilityToggle.onValueChanged.RemoveAllListeners();

        //Sync Toggles
        godModeToggle.isOn = CheatManager.Instance.godModeEnabled;
        speedBoostToggle.isOn = CheatManager.Instance.speedBoostEnabled;
        invulnerabilityToggle.isOn = CheatManager.Instance.invulnerabilityEnabled;

        //Toggles Clicked OFF
        godModeToggle.interactable = CheatManager.Instance.godModeEnabled;
        speedBoostToggle.interactable = CheatManager.Instance.speedBoostEnabled;
        invulnerabilityToggle.interactable = CheatManager.Instance.invulnerabilityEnabled;

        //Reattach Listeners
        godModeToggle.onValueChanged.AddListener(OnGodModeToggle);
        speedBoostToggle.onValueChanged.AddListener(OnSpeedBoostToggle);
        invulnerabilityToggle.onValueChanged.AddListener(OnInvulnerabilityToggle);
    }
}