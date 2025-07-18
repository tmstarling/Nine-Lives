using UnityEngine;
using UnityEngine.UI;

public class gamemanager : MonoBehaviour
{
    public static gamemanager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPaused;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;

    public Image playeHPBar;
    public GameObject playerDamagePanel;
    public bool isPaused;
    public GameObject player;
    public PlayerController playerScript;

    float timescaleOrig;

    int gameGoalCount;

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

    public void updateGameGoal(int amount)
    {
        gameGoalCount = amount; 
        if (gameGoalCount <= 0) 
        { statePaused(); 
          menuActive = menuWin; 
          menuActive.SetActive(true); 
        }
    }

    

    public void youLose() 
    {  
        statePaused();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }   
}
