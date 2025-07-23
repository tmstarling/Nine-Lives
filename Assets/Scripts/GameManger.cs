using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gamemanager : MonoBehaviour
{
    public static gamemanager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPaused;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] TMP_Text gameGoalCountText;

    public Image playeHPBar;
    public GameObject playerDamagePanel;
    public bool isPaused;
    public GameObject player;
    public PlayerController playerScript;
    public GameObject InteractButton;

    float timescaleOrig;

    int gameGoalCount;
    public static int amount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;

        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        timescaleOrig = Time.timeScale;
        gameGoalCount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
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
        Time.timeScale = timescaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }

    public void updateGameGoal()
    {
        gameGoalCount = amount;
        gameGoalCountText.text = gameGoalCount.ToString("F0");
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
        Debug.Log("Player entered the litter box - they Win!");
        youWin();
    }
}
