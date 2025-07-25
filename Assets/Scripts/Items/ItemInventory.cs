// ItemInventory.cs
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct ItemCountPair
{
    public WeaponBuffItem item;
    public int count;
}

public class ItemInventory : MonoBehaviour
{
    [Header("Equipped Items")]
    public List<WeaponBuffItem> equippedItems = new List<WeaponBuffItem>();

    [Header("Item Settings")]
    [Range(0, 10)]
    [SerializeField]
    private int maxPerItems = 3;  // Máximo de veces que puede equiparse cada ítem

    [Header("Item Counts (read-only)")]
    [Tooltip("Cada par muestra un prefab de ítem y cuántas veces aparece en 'equippedItems'")]
    public List<ItemCountPair> itemCountsList = new List<ItemCountPair>();

    // Diccionario interno para acceso rápido en código
    private Dictionary<WeaponBuffItem, int> itemCounts = new Dictionary<WeaponBuffItem, int>();

    void Awake()
    {
        BuildItemCounts();
    }

    /// <summary>
    /// Recorre `equippedItems`, cuenta duplicados (hasta maxPerItems),
    /// rellena el diccionario interno y la lista serializable para el Inspector.
    /// </summary>
    public void BuildItemCounts()
    {
        itemCounts.Clear();

        foreach (var item in equippedItems)
        {
            if (item == null)
                continue;

            // si ya existe y no supera el máximo, incrementa; si no existe, crea la entrada
            if (itemCounts.TryGetValue(item, out int current))
            {
                if (current < maxPerItems)
                    itemCounts[item] = current + 1;
            }
            else
            {
                itemCounts.Add(item, 1);
            }
        }

        // Volcar el diccionario a la lista para mostrar en el Inspector
        itemCountsList.Clear();
        foreach (var kvp in itemCounts)
        {
            itemCountsList.Add(new ItemCountPair {
                item = kvp.Key,
                count = kvp.Value
            });
            //Debug.Log($"{kvp.Key.name} → {kvp.Value}");
        }
    }

    public bool tryAddItem(WeaponBuffItem item)
    {
        if (item == null) return false;

        // Si el ítem ya está equipado, no lo añade
        // if (equippedItems.Contains(item)) return;

        // Si el ítem no supera el máximo, lo añade
        if (itemCounts.TryGetValue(item, out int currentCount) && currentCount < maxPerItems)
        {
            equippedItems.Add(item);
            AudioManager.Instance.PlayPickupSound();  
            
            BuildItemCounts();  // Actualiza los conteos después de añadir
            return true;
        }
        else if (!itemCounts.ContainsKey(item) || currentCount < maxPerItems)
        {
            equippedItems.Add(item);
            BuildItemCounts();  // Actualiza los conteos después de añadir
            return true;

        }
        else
        {
            Debug.LogWarning($"No se puede añadir {item.name}: máximo alcanzado ({maxPerItems})");
            return false;
        }
    }
    public WeaponStats ApplyBuffs(WeaponStats baseStats)
    {
        WeaponStats stats = baseStats.Clone();
        foreach (var item in equippedItems)
        {
            stats.damage = stats.damage * item.damageMultiplier + item.damageBonus;
            stats.fireRate = stats.fireRate * item.fireRateMultiplier + item.fireRateBonus;
            stats.projectileSpeed = stats.projectileSpeed * item.projectileSpeedMultiplier + item.projectileSpeedBonus;
            stats.clipSize = stats.clipSize * item.clipSizeMultiplier + item.clipSizeBonus;
        }
        return stats;
    }
}