using UnityEngine;
using UnityEngine.AI;

public class SniperEnemy : Enemy
{
    [Header("Sniper Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 25f;
    public float zoomedFOV = 30f;
    public float unzoomedFOV = 60f;

    protected override void DoAttack()
    {
        if (bulletPrefab == null || firePoint == null) return;

        // PER-ATTACK DAMAGE INDICATOR (Option 1)
        if (!DI_system.Instance.IsTargetVisible(transform))
        {
            DI_system.Instance.CreateIndicator(transform);
        }

        Vector3 dir = (player.position - firePoint.position).normalized;
        
        GameObject b = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(dir));
        SniperBullet bullet = b.GetComponent<SniperBullet>();
        
        if (bullet != null)
        {
            bullet.speed = bulletSpeed;
            bullet.damage = attackPower;
            bullet.direction = dir;
            bullet.owner = gameObject;  // CRITICAL: Set owner reference
        }

        Rigidbody rb = b.GetComponent<Rigidbody>();
        if (rb != null) rb.linearVelocity = dir * bulletSpeed;
    }

    protected override void Update()
    {
        // Handle camera zoom
        if (currentState == State.Chase || currentState == State.Attack)
        {
            Camera.main.fieldOfView = zoomedFOV;
        }
        else 
        {
            Camera.main.fieldOfView = unzoomedFOV;
        }

        base.Update();
    }
}