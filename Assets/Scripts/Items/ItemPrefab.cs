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
            Debug.Log("Item picked up!");
            inventory = other.GetComponentInChildren<ItemInventory>();
            if (inventory == null)
            {
                Debug.LogWarning("No inventory found on player!");
                return;}
            inventory.equippedItems.Add(itemPrefab);    

            Destroy(gameObject); // Destroy the item after pickup
        }
    }
}
