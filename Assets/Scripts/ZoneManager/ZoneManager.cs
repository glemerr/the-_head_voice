using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;
public class ZoneManager : MonoBehaviour
{
    public static ZoneManager Instance;

    [Header("References")]
    public List<ZoneTrigger> zonesMap = new List<ZoneTrigger>();
    public EnemyManager enemyManager;
    public ZoneUIManager uiZoneManager;

    public GameObject wallPrefab;
    public float activationDelay = 3f;
    [Header("Zone State")]
    public int EnemiesRemaining { get; private set; }

    private ZoneTrigger activeZone;
    private float zoneTimer;
    private bool isZoneActive;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }


    }

    void Start()
    {
        enemyManager.Initialize(this);

        foreach (ZoneTrigger zoneTrigger in zonesMap)
        {
            zoneTrigger.Initialize(this);
            if (zoneTrigger.perimeterSpawner == null)
            {
                zoneTrigger.perimeterSpawner = zoneTrigger.gameObject.AddComponent<PerimeterSpawner>();
            }
        }
    }

    public void ActivateZone(ZoneTrigger zone)
    {
        if (isZoneActive)
        {
            CleanUpCurrentZone();
        }
        if (activeZone != null && activeZone.isActive)
        {
            Debug.LogWarning("A zone is already active. Deactivating it first.");
            DeactivateZone(activeZone);
        }
        activeZone = zone;
        isZoneActive = true;

        AudioManager.Instance.PlayMusic();
        StartCoroutine(delay(activationDelay, zone));

        Zone zoneConfig = activeZone.currentZone;

        // Initialize zone
        zoneTimer = zoneConfig.maxTime;
        EnemiesRemaining = zoneConfig.totalEnemies;

        // Configure enemies
        enemyManager.ConfigureForZone(zoneConfig);
        enemyManager.StartSpawning();

        // Start UI sequence
        uiZoneManager.StartZoneSequence(zoneConfig);
        uiZoneManager.UpdateEnemyCount(zoneConfig.totalEnemies - EnemiesRemaining, zoneConfig.totalEnemies);


        // // Spawn perimeter walls
        // perimeterSpawner.SpawnFaces(zone, wallPrefab);
    }

    public void DeactivateZone(ZoneTrigger zone)
    {
        if (activeZone != zone || !isZoneActive) return;
        AudioManager.Instance.StopMusic();
        CleanUpCurrentZone();
        uiZoneManager.EndZoneSequence(false, activeZone.currentZone);
    }

    void Update()
    {
        if (!isZoneActive) return;

        // Update timer
        zoneTimer -= Time.deltaTime;
        uiZoneManager.UpdateTimer(zoneTimer);

        // Check for failure
        if (zoneTimer <= 0)
        {
            DeactivateZone(activeZone);
            return;
        }

        // Check for completion
        if (EnemiesRemaining <= 0)
        {
            EndZoneSuccessfully();
        }
    }

    public void OnEnemyDefeated()
    {
        if (!isZoneActive) return;

        EnemiesRemaining--;
        AudioManager.Instance.PlayEnemyDeathSound();
        uiZoneManager.UpdateEnemyCount(
            activeZone.currentZone.totalEnemies - EnemiesRemaining,
            activeZone.currentZone.totalEnemies
        );
    }

    private void EndZoneSuccessfully()
    {
        CleanUpCurrentZone();
        uiZoneManager.EndZoneSequence(true, activeZone.currentZone);
    }

    private void CleanUpCurrentZone()
    {
        if (!isZoneActive) return;

        enemyManager.StopSpawning();
        enemyManager.ClearAllEnemies();
        isZoneActive = false;
        activeZone.isZonefinished = true;
        // Destroy perimeter walls
        if (activeZone != null && activeZone.perimeterSpawner != null)
        {
            activeZone.perimeterSpawner.ClearFaces();
        }
    }

    IEnumerator delay(float delay, ZoneTrigger zone)
    {
        yield return new WaitForSeconds(delay);


        // Zone zoneConfig = activeZone.currentZone;

        // // Initialize zone
        // zoneTimer = zoneConfig.maxTime;
        // EnemiesRemaining = zoneConfig.totalEnemies;

        // // Configure enemies
        // enemyManager.ConfigureForZone(zoneConfig);
        // enemyManager.StartSpawning();

        // // Start UI sequence
        // uiZoneManager.StartZoneSequence(zoneConfig);
        // uiZoneManager.UpdateEnemyCount(zoneConfig.totalEnemies - EnemiesRemaining, zoneConfig.totalEnemies);


        // Spawn perimeter walls
        zone.perimeterSpawner.SpawnFaces(zone, wallPrefab);
    }
    public void AddTime(float timeAmount)
    {
        Debug.Log($"Adding {timeAmount} seconds to zone timer in {activeZone?.currentZone?.zoneName ?? "unknown zone"}");
        if (activeZone == null || !isZoneActive) return;

        float time = timeAmount;
        if (time <= 0) return;

        // Clamp to max time
        if (zoneTimer + time > activeZone.currentZone.maxTime)
        {
            time = activeZone.currentZone.maxTime - zoneTimer;
        }

        // Add to timer
        zoneTimer += time;
    }

}