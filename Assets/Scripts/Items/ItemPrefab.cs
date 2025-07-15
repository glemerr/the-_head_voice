using UnityEngine;

public class ItemPrefab : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Item Settings")]
    private ItemInventory inventory;

    public WeaponBuffItem itemPrefab;
    [Header("Pickup Settings")]
    public float pickupRange = 5f;
    public float pickupForce = 100f;

    private void Awake()
    {

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider is an item pickup target.
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Item picked up!");
            inventory = other.GetComponentInChildren<ItemInventory>();
            if (inventory == null)
            {
                Debug.LogWarning("No inventory found on player!");
                return;}

            if (inventory.tryAddItem(itemPrefab))
            {
                //Debug.Log($"Item {itemPrefab.itemName} added to inventory.");
                NotificationManager.Instance.ShowItemNotification(
                itemPrefab.itemName,
                itemPrefab.itemDescription,
                itemPrefab.itemIcon
            );

                NotificationManager.Instance.ShowItemNotification(
                    "Press 'I' ",
                    "Press 'I' to open inventory ",
                itemPrefab.itemIcon
            );
                Destroy(gameObject); // Destroy the item after pickup
            }
            else
            {
                //Debug.LogWarning($"Failed to add item {itemPrefab.itemName} to inventory. It may already exist.");
                NotificationManager.Instance.ShowItemNotification(
                "Failed to add item", 
                $"{itemPrefab.itemName} to inventory. iNnventory is full.", 
                itemPrefab.itemIcon
            );
            }

        }
    }
}
