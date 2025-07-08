using UnityEngine;

public class gameManger : MonoBehaviour
{
    public static gameManger instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPaused;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;


    public bool isPaused = false;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            isPaused = isPaused;
            menuPaused.SetActive(isPaused);
        }
        
    }
}
