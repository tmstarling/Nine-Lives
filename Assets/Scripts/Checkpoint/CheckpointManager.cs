using UnityEngine;
using UnityEngine.SceneManagement;

// this is to make sure game manager is initialized first, since we access its properties in awake.
[DefaultExecutionOrder(1000000)]
public class CheckpointManager : MonoBehaviour
{
    int _score;
    int score {
        get => _score;
        set
        {
            _score = value;
            SetScoreText();
        }
    }
    int lastScore;

    void SetScoreText()
    {
        if (gamemanager.instance == null || gamemanager.instance.gameScoreText == null)
            return;

        gamemanager.instance.gameScoreText.text = $"Score: {score}";
    }

    public void AddScore(int score)
    {
        this.score += score;
    }
    public void SetScore(int score)
    {
        this.score = score;
    }

    public int GetScore() => score;

    public static CheckpointManager instance { get; private set; }
    Vector3 playerSpawnPoint;

    int checkpointID;

    public int GetCheckpointID() => checkpointID;

    public void UpdateCheckpoint(int id, Vector3 pos)
    {
        checkpointID = id;
        playerSpawnPoint = pos;
        lastScore = score;
    }

    private void Awake()
    {
        transform.parent = null;
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return; 
        }
        instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name == "Main Menu")
        { 
            return;
        }

        SetScoreText();
        var spawnPos = GameObject.FindGameObjectWithTag("StartPosition");
        if (spawnPos != null)
            playerSpawnPoint = spawnPos.transform.position;
        else
            playerSpawnPoint = gamemanager.instance.player.transform.position;
        MovePlayerToLastSpawn();
    }

    void MovePlayerToLastSpawn()
    {
        gamemanager.instance.player.transform.position = playerSpawnPoint;
    }

    public void ResetToLastCheckpoint()
    {
        GameObject.FindWithTag("Player").transform.position = playerSpawnPoint;
        if (ObjectiveManager.instance != null)
            ObjectiveManager.instance.SkipTo(checkpointID);
        score = lastScore;
    }
}
