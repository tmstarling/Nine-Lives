using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject prefabToSpawn;
    [SerializeField] int maxSpawns;
    [SerializeField] int spawnRate;
    [SerializeField] float timeBetweenSpawns;

    [SerializeField] Transform[] spawnPoints;

    float nextSpawnTime;
    int currentSpawnCount;
    bool canSpawn = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (canSpawn)
        {
            nextSpawnTime = Time.deltaTime;

            if (nextSpawnTime >= spawnRate && currentSpawnCount < maxSpawns)
            {
                spawnObject();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canSpawn = true;
        }
    }

    void spawnObject()
    {
        int arrayPos = Random.Range(0, spawnPoints.Length);

        Instantiate(prefabToSpawn, spawnPoints[arrayPos].transform.position, spawnPoints[arrayPos].transform.rotation);
        currentSpawnCount++;
        nextSpawnTime = 0;
    }
}