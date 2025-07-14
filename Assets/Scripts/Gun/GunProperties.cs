
using UnityEngine;

[CreateAssetMenu(fileName = "GunProperties_", menuName = "FPS/Gun Properties", order = 1)]
public class GunProperties : ScriptableObject
{
    [Header("General")]
    public string weaponName;
    public KeyCode activationKey = KeyCode.Alpha1;

    [Header("Firing")]
    public float fireRate = 0.2f;
    public int clipSize = 12;
    public float damage = 10f;

    [Header("Projectile")]
    public GameObject projectilePrefab;
    //public GameObject  firePoint;
    public float projectileSpeed = 50f;
    public float cooldown;

    [Header("Trajectory")]
    public float timeToLive = 5f;
    public int trajectoryResolution = 2;
    public float speed;
    //private LineRenderer lineRenderer;
    //private Camera playerCamera;
    public float gravity = -9.81f;
}