using UnityEngine;
using UnityEngine.SceneManagement;

// this is to make sure game manager is initialized first, since we access its properties in awake.
[DefaultExecutionOrder(1000000)]
public class CheckpointManager : MonoBehaviour
{
    static bool awoken = false;
    public static CheckpointManager instance { get; private set; }
    Vector3 playerSpawnPoint;
    Transform player => gamemanager.instance.player.transform;

    int checkpointID;

    public void UpdateCheckpoint(int id, Vector3 pos)
    {
        checkpointID = id;
        playerSpawnPoint = pos;
    }

    private void Awake()
    {
        if (awoken)
            return;
        DontDestroyOnLoad(gameObject);
        instance = this;
        var spawnPos = GameObject.FindGameObjectWithTag("StartPosition");
        if (spawnPos != null)
            playerSpawnPoint = spawnPos.transform.position;
        else
            playerSpawnPoint = Vector3.zero;
        MovePlayerToLastSpawn();
        awoken = true;
    }

    void MovePlayerToLastSpawn() => player.position = playerSpawnPoint;

    public async void ResetToLastCheckpoint()
    {
        await SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        GameObject.FindWithTag("Player").transform.position = playerSpawnPoint;
        ObjectiveManager.instance.SkipTo(checkpointID);
    }
}
