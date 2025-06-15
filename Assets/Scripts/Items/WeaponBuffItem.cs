// WeaponBuffItem.cs
using UnityEngine;

[CreateAssetMenu(menuName = "FPS/Items/WeaponBuffItem")]
public class WeaponBuffItem : ScriptableObject
{
    [Header("Multipliers (x)")]
    public float damageMultiplier = 1f;
    public float fireRateMultiplier = 1f;
    public float projectileSpeedMultiplier = 1f;
    public int clipSizeMultiplier = 1;

    public float reloadTimeMultiplier = 1f;

    [Header("Additives (+)")]
    public float damageBonus = 0f;
    public float fireRateBonus = 0f;
    public float projectileSpeedBonus = 0f;
    public int clipSizeBonus = 0;
    public float reloadTimeBonus = 0f;
}
