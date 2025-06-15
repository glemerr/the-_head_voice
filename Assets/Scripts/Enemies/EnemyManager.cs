using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EnemySpawnData
{
    [Tooltip("Enemy Prefab reference")]
    public GameObject enemyPrefab;
    
    [Tooltip("Number of enemies of this type to spawn")]
    public int enemyCount;
}

public class EnemyManager : MonoBehaviour
{
    [Header("Enemy Types")]
    [Tooltip("List of enemy types and the count to spawn for each type")]
    public List<EnemySpawnData> enemySpawnList = new List<EnemySpawnData>();

    [Header("Spawn Settings")]
    public Transform[] spawnPoints;
    public float spawnInterval = 10f;
    public int maxEnemies = 10;

    private List<GameObject> activeEnemies = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(SpawnEnemiesRoutine());
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            CleanUpNullEnemies();

            // Check if we can spawn more
            if (activeEnemies.Count >= maxEnemies) continue;

            foreach (EnemySpawnData spawnData in enemySpawnList)
            {
                // Check if prefab is set
                if (spawnData.enemyPrefab == null)
                {
                    Debug.LogError("Missing enemy prefab in enemySpawnList!");
                    continue;
                }

                // Determine how many of this type to spawn without exceeding maxEnemies
                int spawnableCount = Mathf.Min(spawnData.enemyCount, maxEnemies - activeEnemies.Count);

                for (int i = 0; i < spawnableCount; i++)
                {
                    if (spawnPoints.Length == 0)
                    {
                        Debug.LogError("No spawn points set!");
                        yield break;
                    }
                    
                    // Select random spawn point
                    Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
                    GameObject enemy = Instantiate(spawnData.enemyPrefab, sp.position, sp.rotation);

                    activeEnemies.Add(enemy);
                }
            }
        }
    }

    // Remove destroyed (null) enemies from the active list.
    void CleanUpNullEnemies()
    {
        activeEnemies.RemoveAll(e => e == null);
    }
}