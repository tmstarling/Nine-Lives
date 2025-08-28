using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gamemanager : MonoBehaviour
{
    public static gamemanager instance;

    [Header("Menus")]
    [SerializeField] public GameObject menuActive;
    [SerializeField] GameObject menuPaused;
    [SerializeField] GameObject menuCheat;
    [SerializeField] public GameObject menuAudio;
    [SerializeField] GameObject menuCredits;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    public TextMeshProUGUI gameObjectiveText;
    public TextMeshProUGUI gameScoreText;
    [Header("UI Elements")]
    public Image playeHPBar;
    public GameObject playerDamagePanel;
    public bool isPaused;
    public GameObject player;
    public PlayerController playerScript;
    public GameObject InteractButton;
    public GameObject cheatPopup;

    [Header("Game Settings")]
    int pickUpsCount = 0;
    public static int amount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;

        if (IsGameplayScene())
        {
            player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerScript = player.GetComponent<PlayerController>();
            }
            else
            {
                Debug.LogWarning("Player not found in gameplay scene.");
            }

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    bool IsGameplayScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        return sceneName != "Main Menu";
    }

    // Update is called once per frame
    void Update()
    {
#if !UNITY_WEBGL
        bool keycodedown = Input.GetKeyDown(KeyCode.Escape);
#else
        bool keycodedown = Input.GetKeyDown(KeyCode.P);
#endif
        if (keycodedown)
        {
           if(menuActive == null)
            {
                statePaused();
                menuActive = menuPaused;
                menuActive.SetActive(true);

            }
            else if (menuActive == menuPaused)
            {
                stateUnpaused();
            }
        }
    }

    public void statePaused()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void stateUnpaused()
    {
        isPaused = false;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (menuActive != null)
            menuActive.SetActive(false);
        menuActive = null;
    }

    public void youCheat()
    {
        if (menuActive != null)
        {
            menuActive.SetActive(false);
        }
        menuActive = menuCheat;
        menuActive.SetActive(true);
    }

    public void audioMixer()
    {
        if (menuActive != null)
        {
            menuActive.SetActive(false);
        }

        menuActive = menuAudio;
        menuActive.SetActive(true);
    }

    public void credits()
    {
        if (menuActive != null)
        {
            menuActive.SetActive(false);
        }
        menuActive = menuCredits;
        menuActive.SetActive(true);
    }

    public void youLose() 
    {  
        statePaused();
        menuActive = menuLose;
        menuActive.SetActive(true);
    } 
    
    public void youWin()
    {
        statePaused();
        menuActive = menuWin;
        menuActive.SetActive(true);
    }

    public void PlayerEnteredLitterBox()
    {
     
        youWin();
    }
}
