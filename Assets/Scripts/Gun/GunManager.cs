using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
[RequireComponent(typeof(LineRenderer))]
public class GunManager : MonoBehaviour
{
    [Header("Todas las armas disponibles")]
    public List<GunBase> allGuns;    // Lista maestra con todas las armas del juego

    [Header("Armas que podrá usar el jugador")]
    public List<GunBase> guns; 

    [Header("Fire Point")]
    private Transform firePoint;

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
    public bool isPaused = false;


    private GameObject gunModelInstance;

    [Header("Switch Settings")]
    public float switchGunTime = 0.5f;
    private bool isSwitching = false;
    private int activeGunIndex = -1;

    public int MaxGuns = 2;

    void Awake()
    {
        // Cada vez que se cargue el GunManager, seleccionamos entre 1 y 2 armas al azar
        SelectRandomGuns();
        


    }

    private void SelectRandomGuns()
    {
        int maxPosibles = Mathf.Min(MaxGuns, allGuns.Count);
        int numeroDeArmas = Random.Range(1, maxPosibles + 1); // 1 ó 2

        // Con LINQ: barajea y toma las primeras n
        guns = allGuns
            .OrderBy(_ => Random.value)
            .Take(numeroDeArmas)
            .ToList();

    }

    void Start()
    {
        playerCamera = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();

        if (inventory == null)
            inventory = GetComponent<ItemInventory>();

        if (guns.Count > 0)
            EquipGun(Random.Range(0, guns.Count));
        firePoint = gunModelInstance.transform.Find("Launch");
        if (firePoint == null)
        {
            Debug.LogError("Fire Point not found! Please assign a Fire Point in the GunManager.");
        }      
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
        if ( Input.GetMouseButton(0) && cooldownTimer <= 0f && ammoInClip > 0 && !isPaused)
        {
            ApplyBuffsToActiveGun();
            activeGun.Fire(firePoint, playerCamera, lineRenderer);
            cooldownTimer = activeGun.fireRate;
            ammoInClip--;
        }

        // Reload
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && ammoInClip < activeGun.clipSize && !isPaused)
        {
            StartReload();
        }
    }

    void SwitchGuns()
    {
        for (int i = 0; i < guns.Count && i < 9; i++)
        {
            //Debug.Log(KeyCode.Alpha1 + i);
            if (i != activeGunIndex && Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                // Inicia la rutina de cambio
                StartCoroutine(DelayedEquip(i));
                break;
            }
        }
    }


    public IEnumerator DelayedEquip(int index)
    {
        isSwitching = true;
        // (Opcional) aquí podrías reproducir animación de sacar/guardar arma
        yield return new WaitForSeconds(switchGunTime);
        EquipGun(index);
        isSwitching = false;
    }
    public void EquipGun(int index)
    {
        if (index < 0 || index >= guns.Count) return;
        // Destroy previous gun model
        if (gunModelInstance != null)
        {
            Destroy(gunModelInstance);
        }

        // Assign and reset gun stats
        activeGun = guns[index];
        activeGun.ResetRuntimeStats();
        activeGunIndex = index;
        // Instantiate new gun model
        if (activeGun.gunModelPrefab != null && gunSpawnModel != null)
        {
            gunModelInstance = Instantiate(activeGun.gunModelPrefab, gunSpawnModel);
            gunModelInstance.transform.localPosition = Vector3.zero;
            //gunModelInstance.transform.localRotation = Quaternion.identity;
            // gunModelInstance.transform.localScale = Vector3.one;
            firePoint = gunModelInstance.transform.Find("Launch");
            if (firePoint == null)
            {
                Debug.LogError("Fire Point not found! Please assign a Fire Point in the GunManager.");
            }
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
