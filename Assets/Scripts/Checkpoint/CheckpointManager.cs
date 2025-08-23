using UnityEngine;

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

    void SetScoreText() => gamemanager.instance.gameScoreText.text = string.Format("Score: {0}", score);
    
    public void AddScore(int score)
    {
        this.score += score;
    }
    public void SetScore(int score)
    {
        this.score = score;
    }

    public int GetScore() => score;

    static bool awoken = false;
    public static CheckpointManager instance { get; private set; }
    Vector3 playerSpawnPoint;
    Transform player => gamemanager.instance.player.transform;

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
        if (awoken)
            return;
        var spawnPos = GameObject.FindGameObjectWithTag("StartPosition");
        if (spawnPos != null)
            playerSpawnPoint = spawnPos.transform.position;
        else
            playerSpawnPoint = Vector3.zero;
        MovePlayerToLastSpawn();
        DontDestroyOnLoad(gameObject);
        awoken = true;
    }

    private void OnLevelWasLoaded(int level)
    {
        SetScoreText();
    }

    void MovePlayerToLastSpawn() => player.position = playerSpawnPoint;

    public void ResetToLastCheckpoint()
    {
        GameObject.FindWithTag("Player").transform.position = playerSpawnPoint;

        while (ObjectiveManager.instance == null)
        {
            await System.Threading.Tasks.Task.Yield();
        }

        ObjectiveManager.instance.SkipTo(checkpointID);
        score = lastScore;
    }
}
