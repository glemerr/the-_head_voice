using UnityEngine;
using UnityEngine.AI;

public class SniperEnemy : Enemy
{
    [Header("Sniper Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 25f;
    public float zoomedFOV = 30f;    // optional: camera‐zoom effect  
    public float unzoomedFOV = 60f;

protected override void DoAttack()
{
    if (bulletPrefab == null || firePoint == null) return;

    // Calculate direction from the firePoint to the player's head level
    Vector3 dir = (player.position + Vector3.up  - firePoint.position).normalized;

    // Optional: draw ray to debug
    Debug.DrawRay(firePoint.position, dir * attackRange, Color.red, 1f);

    // Instantiate and fire bullet regardless of raycast hit
    GameObject b = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(dir));
    Bullet bullet = b.GetComponent<Bullet>();
    if (bullet != null) bullet.speed = bulletSpeed;
    bullet.damage = attackPower; // Set damage for bullet logic
    bullet.direction = dir; // Set bullet direction

    Rigidbody rb = b.GetComponent<Rigidbody>();
    if (rb != null) rb.linearVelocity = dir * bulletSpeed;

    //Debug.Log("Fired " + bulletSpeed + " " + bulletPrefab.name + " Dir " + dir);
}

    protected override void Update()
    {
        // Optional: zoom in while aiming
        if (currentState == State.Chase || currentState == State.Attack)
        {
            Camera.main.fieldOfView = zoomedFOV;
        }
        else Camera.main.fieldOfView = unzoomedFOV;

        base.Update();
    }
}
