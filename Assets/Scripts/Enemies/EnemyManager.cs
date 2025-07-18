using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab;
    public int enemyCount;
}

public class EnemyManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Transform[] spawnPoints;
    public float defaultSpawnInterval = 5f;
    public int defaultMaxEnemies = 10;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private Coroutine spawnCoroutine;
    private Zone currentZone;
    private int totalEnemiesToSpawn;
    private int enemiesSpawned;
    private ZoneManager zoneManager;

    public void Initialize(ZoneManager manager)
    {
        zoneManager = manager;
    }

    public void ConfigureForZone(Zone zoneConfig)
    {
        currentZone = zoneConfig;
        totalEnemiesToSpawn = zoneConfig.totalEnemies+10;
        enemiesSpawned = 0;
    }

    public void StartSpawning()
    {
        if (spawnCoroutine != null) 
            StopCoroutine(spawnCoroutine);
        
        spawnCoroutine = StartCoroutine(SpawnEnemiesRoutine());
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        while (enemiesSpawned < totalEnemiesToSpawn)
        {
            yield return new WaitForSeconds(currentZone.enemySpawnRate);
            
            CleanUpNullEnemies();
            if (activeEnemies.Count >= currentZone.maxConcurrentEnemies) 
                continue;

            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points defined!");
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(
            currentZone.enemyTypes[Random.Range(0, currentZone.enemyTypes.Count)].enemyPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );
        //Debug.Log($"Spawned enemy: {enemy.name} at {spawnPoint.position}");
        // Configure enemy stats
        Enemy controller = enemy.GetComponent<Enemy>();
        LifeSystem Deathcontroller = enemy.GetComponent<LifeSystem>();

        if (controller)
        {
            controller.SetStats(
                currentZone.healthMultiplier,
                currentZone.damageMultiplier
            );
        Deathcontroller.OnDeath.AddListener(() => HandleEnemyDeath(enemy));
        }
        else
        {
            Debug.LogWarning("Spawned enemy does not have an Enemy component!");
        }

        activeEnemies.Add(enemy);
        enemiesSpawned++;
    }

public void HandleEnemyDeath(GameObject enemy)
{
    if (!activeEnemies.Contains(enemy)) return;

    activeEnemies.Remove(enemy);
    zoneManager.OnEnemyDefeated();
}

    public void ClearAllEnemies()
    {
        StopSpawning();
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
            {
                LifeSystem controller = enemy.GetComponent<LifeSystem>();
                if (controller) controller.OnDeath.RemoveListener(() => HandleEnemyDeath(enemy));
                Destroy(enemy);
            }
        }
        activeEnemies.Clear();
    }

    void CleanUpNullEnemies()
    {
        activeEnemies.RemoveAll(e => e == null);
    }
}