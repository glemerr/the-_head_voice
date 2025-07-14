using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ZoneManager : MonoBehaviour
{
    public static ZoneManager Instance;

    [Header("References")]
    public List<ZoneTrigger> zonesMap = new List<ZoneTrigger>();
    public EnemyManager enemyManager;
    public ZoneUIManager uiZoneManager;


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
        }
    }

    public void ActivateZone(ZoneTrigger zone)
    {
        if (isZoneActive)
        {
            DeactivateZone(activeZone);
        }

        activeZone = zone;
        isZoneActive = true;
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
    }

    public void DeactivateZone(ZoneTrigger zone)
    {
        if (activeZone != zone || !isZoneActive) return;

        enemyManager.StopSpawning();
        enemyManager.ClearAllEnemies();
        isZoneActive = false;

        // End UI sequence with failure
        uiZoneManager.EndZoneSequence(false, activeZone.currentZone);
    }

    void Update()
    {
        if (!isZoneActive) return;

        // Update timer
        zoneTimer -= Time.deltaTime;
        uiZoneManager.UpdateTimer(zoneTimer);
        //uiZoneManager.;
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

        uiZoneManager.UpdateEnemyCount(
        activeZone.currentZone.totalEnemies - EnemiesRemaining,
        activeZone.currentZone.totalEnemies
        );
    }

    private void EndZoneSuccessfully()
    {
        isZoneActive = false;
        enemyManager.StopSpawning();

        // End UI sequence with success
        uiZoneManager.EndZoneSequence(true, activeZone.currentZone);
    }
    


}