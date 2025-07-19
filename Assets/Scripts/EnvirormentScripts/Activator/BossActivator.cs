using UnityEngine;
using System.Collections.Generic;
public class BossActivator : MonoBehaviour
{
    [SerializeField] private GameObject zoneBoss;
    private ZoneTrigger zoneTrigger;
    private InventoryManager inventoryManager;
    private ZoneManager zoneManager;
    private List<GameObject> spawnedPortals = new List<GameObject>();
    [SerializeField] private GameObject portalPrefab;
    
    [Header("Portal Settings")]
    [SerializeField] private float portalRotationSpeed = 30f;
    [SerializeField] private float portalActivationDelay = 1.0f;
    [SerializeField] private GameObject portalEffect;
    [SerializeField] private AudioClip portalSound;
    [SerializeField] private List<Transform> portalSpawnPoints = new List<Transform>();
    void Start()
    {
        zoneBoss.SetActive(false);
        zoneManager = ZoneManager.Instance;
        zoneTrigger = zoneBoss.GetComponentInChildren<ZoneTrigger>();
        if (zoneManager == null)
        {
            Debug.LogError("BossActivator is missing ZoneTrigger or ZoneManager references.");
            return;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventoryManager = other.GetComponent<InventoryManager>();

            if (zoneTrigger == null || zoneManager == null || inventoryManager == null)
            {
                Debug.LogError("BossActivator is missing required components or references.");
                return;
            }
            ;
            if (inventoryManager == null) return;

            InventorySlot slot = inventoryManager.GetSlot(CollectableType.KeyItem);
            if (slot == null)
            {
                NotificationManager.Instance.ShowMissionNotification(
                    "Debes Encontrar la llave",
                    "Para abiri la puerta entre los muodos"
                );
                return;
            }
            if (slot != null && slot.quantity > 0)
            {
                zoneBoss.SetActive(true);

                if (!zoneManager.zonesMap.Contains(zoneTrigger))
                {
                    zoneManager.zonesMap.Add(zoneTrigger);
                    zoneTrigger.Initialize(zoneManager);
                    NotificationManager.Instance.ShowItemNotification(
                    "Health Potion",
                    "Restores 50 HP",
                    slot.collectable.icon
                );
                }
            }
            if (zoneTrigger != null && zoneTrigger.isZonefinished)
            {
                NotificationManager.Instance.ShowMissionNotification(
                    "Zona Finalizada",
                    "Has completado la zona, puedes continuar"
                );
                SpawnPortals();
                return;
            }
        }
    }
    
    private void SpawnPortals()
    {
        if (portalPrefab == null || portalSpawnPoints.Count == 0)
        {
            Debug.LogError("Portal prefab or spawn points not assigned!");
            return;
        }

        foreach (Transform spawnPoint in portalSpawnPoints)
        {
            GameObject portal = Instantiate(portalPrefab, spawnPoint.position, spawnPoint.rotation);
            spawnedPortals.Add(portal);
            
            // Configurar la rotación del portal
            PortalRotator rotator = portal.GetComponent<PortalRotator>();
            if (rotator != null)
            {
                // Alternar dirección de rotación para cada portal
                bool clockwise = (spawnedPortals.Count % 2 == 1);
                rotator.Initialize(portalRotationSpeed, clockwise);
            }
            
            // Configurar la activación del portal
            PortalSpawner portalScript = portal.GetComponent<PortalSpawner>();
            if (portalScript != null)
            {
                portalScript.Initialize(portalActivationDelay, portalEffect, portalSound);
            }
        }
    }

}