using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner instance;

    [System.Serializable]
    public class ItemDrop
    {
        public GameObject prefab;
        public int count;
    }

    [SerializeField] List<ItemDrop> itemDrops = new List<ItemDrop>();
    [SerializeField] float spacing = 2f;

    void Awake()
    {
        instance = this;
    }

    public void SpawnItem()
    {
        var player = gamemanager.instance?.player;
        if (player == null)
        {
            Debug.LogWarning("Player not found.");
            return;
        }

        Vector3 basePosition = player.transform.position + player.transform.forward * 5f;
        int totalSpawned = 0;

        foreach (var drop in itemDrops)
        {
            if (drop.prefab == null || drop.count <= 0)
                continue;

            for (int i = 0; i < drop.count; i++)
            {
                Vector3 offset = player.transform.right * (totalSpawned - GetTotalItemCount() / 2f) * spacing;
                Vector3 spawnPosition = basePosition + offset;
                Instantiate(drop.prefab, spawnPosition, Quaternion.identity);
                totalSpawned++;
            }
        }
    }

    int GetTotalItemCount()
    {
        int total = 0;
        foreach (var drop in itemDrops)
        {
            total += Mathf.Max(0, drop.count);
        }
        return total;
    }
}
