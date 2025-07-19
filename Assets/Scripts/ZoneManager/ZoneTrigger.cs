using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    public bool isActive = false;
    public bool isZonefinished = false;
    public Zone[] zones;
    public Zone currentZone;
    private ZoneManager zoneManager;

    public Transform[] spawnPoints;
    private EnemyManager enemyManager;

    [Header("Perimeter Settings")]
    public PerimeterSpawner perimeterSpawner;
    public void Initialize(ZoneManager manager)
    {
        zoneManager = manager;
        enemyManager = manager.enemyManager;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActive)
        {
            isActive = true;
            SelectRandomZone();
            zoneManager.ActivateZone(this);
            enemyManager.spawnPoints = spawnPoints;
        }
    }

    // void OnTriggerExit(Collider other)
    // {
    //     Debug.Log($"OnTriggerExit called for {other.name} in zone {currentZone?.zoneName}");
    //     if (other.CompareTag("Player") && isActive)
    //     {
    //         isActive = false;
    //         zoneManager.DeactivateZone(this);
    //     }
    // }

    private void SelectRandomZone()
    {
        if (zones.Length == 0) return;
        int randomIndex = Random.Range(0, zones.Length);
        currentZone = zones[randomIndex];
    }

    private void OnDrawGizmos()
    {
        if (currentZone != null)
        {
            Gizmos.color = currentZone.gizmoColor;
            Vector3 center = transform.position + new Vector3(currentZone.offset.x, 0, currentZone.offset.y);
            Vector3 size = new Vector3(currentZone.size.x, 1, currentZone.size.y);
            Gizmos.DrawWireCube(center, size);
        }
    }
    void Start()
    {
        int count = transform.childCount;
        spawnPoints = new Transform[count];
        for (int i = 0; i < count; i++)
        {
            spawnPoints[i] = transform.GetChild(i);
        }
    }

}