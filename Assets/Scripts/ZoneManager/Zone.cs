using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ZoneConfig/Zone", fileName = "newZone")]
public class Zone : ScriptableObject
{
    [Header("Zone Settings")]
    public string zoneName;
    public Color gizmoColor = Color.cyan;
    

    [Header("Zone Dimensions")]
    public Vector2Int size;
    public Vector2Int offset;

    [Header("Enemy Settings")]
    public int totalEnemies = 30;
    public int maxConcurrentEnemies = 10;
    public float healthMultiplier = 1.0f;
    public float damageMultiplier = 1.0f;
    public List<EnemySpawnData> enemyTypes = new List<EnemySpawnData>();

    [Header("Zone Difficulty")]
    public float enemySpawnRate = 1.0f;
    public float maxTime = 300f;
    public int difficultyLevel = 1;

    [Header("Zone UI text")]
    public string startMessages = "Zone Started!";
    public string progessMessages = "Enemies remaining: {0}";
    public string endMessages = "Zone Ended.";
    public string failMessages = "Zone Failed - Time's up!";
    public string completeMessage = "Zone Completed!";
}