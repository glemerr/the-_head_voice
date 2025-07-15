using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] private List<ItemPrefab> items = new List<ItemPrefab>();
    [Range(0f, 1f)]
 
    [SerializeField] private float spawnProbability = 0.3f;
    [Header("Spawn Chances (must sum ≤ 1)")]
    [Range(0f, 1f)] public float genericChance = 0.2f;
    [Range(0f, 1f)] public float speedChance   = 0.2f;
    [Range(0f, 1f)] public float healthChance  = 0.3f;
    // noSpawnChance = 1 - (genericChance + speedChance + healthChance)

    [Header("Spawn Settings")]
    public Transform defaultSpawnPoint;

    private ItemInventory inventory;
    public GameObject healthPickupPrefab;
    public GameObject speedPickupPrefab;
    public GameObject player;

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
    public void TrySpawnRandomItem(Vector3 position)
    {
        float roll = Random.value;   // [0,1)
        float cumulative = 0f;
        Debug.Log($"ItemManager: Trying to spawn item at {position} with roll {roll}");
        // 1) Genérico
        cumulative += genericChance;
        if (roll < cumulative)
        {
            SpawnGeneric(position);
            //Debug.Log("Generic item spawned at " + position);
            return;
        }

        // 2) Velocidad
        cumulative += speedChance;
        if (roll < cumulative)
        {
            GameObject timeBonus= Instantiate(speedPickupPrefab, position, Random.rotation);
            timeBonus.GetComponent<TimeBonus>().target = player;
            //Debug.Log("Speed item spawned at " + position);
            return;
        }

        // 3) Salud
        cumulative += healthChance;
        if (roll < cumulative)
        {
            GameObject healthC=Instantiate(healthPickupPrefab, position, Random.rotation);
            healthC.GetComponent<ParticleAttractor>().target = player;
            //Debug.Log("Health item spawned at " + position);
            return;
        }
        // 4) Ningún ítem (roll ≥ cumulative): no hacemos nada
    }

    private void SpawnGeneric(Vector3 position)
    {
        if (items.Count == 0) return;
        int idx = Random.Range(0, items.Count);
        Instantiate(
            items[idx], 
            position, 
            Random.rotation
        );
    }
}
