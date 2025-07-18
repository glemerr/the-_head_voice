using UnityEngine;

public class BossActivator : MonoBehaviour
{
    [SerializeField] private GameObject zoneBoss;
    private ZoneTrigger zoneTrigger;
    private InventoryManager inventoryManager;
    private ZoneManager zoneManager;

    void Start()
    {
        zoneBoss.SetActive(false);
        zoneManager = ZoneManager.Instance;
        zoneTrigger = zoneBoss.GetComponentInChildren<ZoneTrigger>();
        if ( zoneManager == null)
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
        }
    }
}