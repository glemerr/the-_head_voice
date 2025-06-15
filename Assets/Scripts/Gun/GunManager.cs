using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class GunManager : MonoBehaviour
{
    [Header("Weapon Assets")]
    public List<GunBase> guns;

    [Header("Fire Point")]
    public Transform firePoint;

    [Header("Gun Model Spawn Point")]
    public Transform gunSpawnModel;

    [Header("Item Inventory")]
    public ItemInventory inventory;

    private Camera playerCamera;
    private LineRenderer lineRenderer;
    private GunBase activeGun;

    private float cooldownTimer = 0f;
    private float reloadTimer = 0f;
    private bool isReloading = false;
    private int ammoInClip = 0;

    private GameObject gunModelInstance;

    void Start()
    {
        playerCamera = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();

        if (inventory == null)
            inventory = GetComponent<ItemInventory>();

        if (guns.Count > 0)
            EquipGun(0);
    }

    void Update()
    {
        if (activeGun == null) return;

        cooldownTimer -= Time.deltaTime;

        if (isReloading)
        {
            reloadTimer -= Time.deltaTime;
            if (reloadTimer <= 0f)
            {
                FinishReload();
            }
            return; // Block firing/input while reloading
        }

        HandleInput();
        SwitchGuns();
    }

    void HandleInput()
    {
        // Fire
        if ((Input.GetKey(KeyCode.E) || Input.GetMouseButton(0)) && cooldownTimer <= 0f && ammoInClip > 0)
        {
            ApplyBuffsToActiveGun();
            activeGun.Fire(firePoint, playerCamera, lineRenderer);
            cooldownTimer = activeGun.fireRate;
            ammoInClip--;
        }

        // Reload
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && ammoInClip < activeGun.clipSize)
        {
            StartReload();
        }
    }

    void SwitchGuns()
    {
        for (int i = 0; i < guns.Count; i++)
        {
            if (Input.GetKeyDown(guns[i].activationKey))
            {
                EquipGun(i);
                break;
            }
        }
    }

    void EquipGun(int index)
    {
        // Destroy previous gun model
        if (gunModelInstance != null)
        {
            Destroy(gunModelInstance);
        }

        // Assign and reset gun stats
        activeGun = guns[index];
        activeGun.ResetRuntimeStats();

        // Instantiate new gun model
        if (activeGun.gunModelPrefab != null && gunSpawnModel != null)
        {
            gunModelInstance = Instantiate(activeGun.gunModelPrefab, gunSpawnModel);
            gunModelInstance.transform.localPosition = Vector3.zero;
            //gunModelInstance.transform.localRotation = Quaternion.identity;
            // gunModelInstance.transform.localScale = Vector3.one;
        }

        ApplyBuffsToActiveGun();

        ammoInClip = activeGun.clipSize;
        cooldownTimer = 0f;
        isReloading = false;
    }

    void StartReload()
    {
        isReloading = true;
        reloadTimer = activeGun.reloadTime;
        // Optionally trigger reload animation or sound here
    }

    void FinishReload()
    {
        ammoInClip = activeGun.clipSize;
        isReloading = false;
    }

    void ApplyBuffsToActiveGun()
    {
        WeaponStats baseStats = new WeaponStats(
            activeGun.baseDamage,
            activeGun.baseFireRate,
            activeGun.baseProjectileSpeed,
            activeGun.baseClipSize,
            activeGun.baseReloadTime
        );

        WeaponStats buffed = inventory.ApplyBuffs(baseStats);

        activeGun.damage = buffed.damage;
        activeGun.fireRate = buffed.fireRate;
        activeGun.projectileSpeed = buffed.projectileSpeed;
        activeGun.clipSize = buffed.clipSize;
        activeGun.reloadTime = buffed.reloadTime;
    }

    // Public accessors
    public GunBase GetActiveGun() => activeGun;
    public int GetAmmoInClip() => ammoInClip;
    public float GetCooldownTime() => cooldownTimer;
    public float GetReloadTime() => activeGun.reloadTime;
    public float GetReloadProgress() => isReloading ? 1f - (reloadTimer / activeGun.reloadTime) : 1f;
    public bool IsReloading() => isReloading;
}
