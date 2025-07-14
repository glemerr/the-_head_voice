using UnityEngine;

public abstract class GunBase : ScriptableObject
{
    [Header("Identification")]
    public string weaponName;
    public KeyCode activationKey = KeyCode.Alpha1;
    public Sprite weaponIcon;

    [Header("Base Stats (do not modify at runtime)")]
    public float baseDamage = 10f;
    public float baseFireRate = 0.2f;
    public float baseProjectileSpeed = 20f;
    public int baseClipSize = 10;
    public float baseReloadTime = 0.5f;

    [Header("Runtime Stats (overwritten on equip)")]
    [HideInInspector] public float damage;
    [HideInInspector] public float fireRate;
    [HideInInspector] public float projectileSpeed;
    [HideInInspector] public int clipSize;
    [HideInInspector] public float reloadTime;

    [Header("Other Settings")]
    //public int clipSize = 10;
    public float gravity = -9.81f;
    public float timeToLive = 5f;
    public int trajectoryResolution = 10;
    public GameObject projectilePrefab;

    public GameObject gunModelPrefab;

    /// <summary>
    /// Copy base stats into runtime stats.
    /// </summary>
    public void ResetRuntimeStats()
    {
        damage = baseDamage;
        fireRate = baseFireRate;
        projectileSpeed = baseProjectileSpeed;
        clipSize = baseClipSize;
        reloadTime = baseReloadTime;
    }

    public abstract void Fire(Transform firePoint, Camera playerCamera, LineRenderer lineRenderer);
}
