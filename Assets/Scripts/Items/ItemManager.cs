using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] private List<ItemPrefab> items = new List<ItemPrefab>();
    [Range(0f, 1f)]
    [SerializeField] private float spawnProbability = 0.2f;

    [Header("Spawn Settings")]
    public Transform defaultSpawnPoint;

    private ItemInventory inventory;

    void Start()
    {
        if (defaultSpawnPoint == null)
        {
            defaultSpawnPoint = transform;
        }
    }

    /// <summary>
    /// Spawns a specific item at the given spawn point.
    /// </summary>
    public void SpawnItem(ItemPrefab itemPrefab, Transform spawnPoint = null)
    {
        if (itemPrefab == null)
        {
            Debug.LogWarning("ItemPrefab is null.");
            return;
        }

        if (spawnPoint == null)
            spawnPoint = defaultSpawnPoint;

        if (inventory == null)
            inventory = spawnPoint.GetComponentInChildren<ItemInventory>();

        ItemPrefab itemInstance = Instantiate(itemPrefab, spawnPoint.position, spawnPoint.rotation);
        itemInstance.transform.SetParent(spawnPoint);

        Debug.Log($"Item spawned: {itemPrefab.name}");
    }

    /// <summary>
    /// Tries to spawn a random item based on probability.
    /// </summary>
    public void TrySpawnRandomItem(Vector3 position)
    {
        if (items == null || items.Count == 0) return;

        float roll = Random.value;
        if (roll > spawnProbability)
        {
            Debug.Log("No item spawned (roll failed).");
            return;
        }

        int randomIndex = Random.Range(0, items.Count);
        ItemPrefab selectedItem = items[randomIndex];

        Instantiate(selectedItem, position, Quaternion.identity);
        Debug.Log($"Random item spawned at {position}: {selectedItem.name}");
    }

    /// <summary>
    /// Public setter for spawn probability.
    /// </summary>
    public void SetSpawnProbability(float newProb)
    {
        spawnProbability = Mathf.Clamp01(newProb);
    }
}
