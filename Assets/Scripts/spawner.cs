using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private int maxSpawns = 10;
    [SerializeField] private float timeBetweenSpawns = 2f;

    [SerializeField] private Transform[] spawnPoints;

    private float nextSpawnTime;
    private int currentSpawnCount;
    private bool canSpawn = false;

    void Start()
    {
        if (gamemanager.instance != null)
        {
            gamemanager.instance.updateGameGoal(maxSpawns);
        }

        nextSpawnTime = Time.time + timeBetweenSpawns;
    }

    void Update()
    {
        if (canSpawn && Time.time >= nextSpawnTime && currentSpawnCount < maxSpawns)
        {
            SpawnObject();
            nextSpawnTime = Time.time + timeBetweenSpawns;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canSpawn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canSpawn = false;
        }
    }

    void SpawnObject()
    {
        if (spawnPoints.Length == 0 || prefabToSpawn == null) return;

        
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedSpawnPoint = spawnPoints[randomIndex];

        
        GameObject spawnedObject = Instantiate(prefabToSpawn, selectedSpawnPoint.position, selectedSpawnPoint.rotation);

        
        spawnedObject.transform.Rotate(0, Random.Range(0, 360), 0);

        currentSpawnCount++;

        
        Debug.Log($"Spawned object {currentSpawnCount}/{maxSpawns} at {selectedSpawnPoint.name}");
    }

    
    public void ResetSpawner()
    {
        currentSpawnCount = 0;
        canSpawn = false;
        nextSpawnTime = Time.time + timeBetweenSpawns;
    }

   
    public bool IsSpawningComplete()
    {
        return currentSpawnCount >= maxSpawns;
    }
}