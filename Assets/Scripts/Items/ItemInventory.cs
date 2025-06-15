// ItemInventory.cs
using UnityEngine;
using System.Collections.Generic;

public class ItemInventory : MonoBehaviour
{
    [Header("Equipped Items")]
    public List<WeaponBuffItem> equippedItems = new List<WeaponBuffItem>();

    /// <summary>
    /// Applies all equipped item buffs to base weapon stats and returns the modified values.
    /// </summary>
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