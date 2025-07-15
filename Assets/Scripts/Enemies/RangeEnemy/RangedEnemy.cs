using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("Ranged Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 12f;

    protected override void DoAttack()
    {
        if (projectilePrefab == null || firePoint == null) return;
            if (!DI_system.Instance.IsTargetVisible(transform))
            {
                DI_system.Instance.CreateIndicator(transform);
            }
        // Instantiate and fire projectile toward player
        Vector3 dir = (player.position + Vector3.up  - firePoint.position).normalized;
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(dir));
        EnemyBombBullet bullet = proj.GetComponent<EnemyBombBullet>();
        if (bullet == null) return;
        bullet.owner = this.gameObject;
        bullet.speed = projectileSpeed;
        bullet.damage = attackPower; // Set damage for the bullet logic
        bullet.direction = dir; // Store direction for bullet logic
        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null) rb.linearVelocity = dir * projectileSpeed;
    }

    protected override void Update()
    {
        base.Update();

        // Add strafing while chasing
        if (currentState == State.Chase)
        {
            Vector3 perp = Vector3.Cross(Vector3.up, (player.position - transform.position).normalized);
            Vector3 strafeTarget = transform.position + perp * Mathf.Sin(Time.time * 2f) * 2f;
            agent.Move((strafeTarget - transform.position) * Time.deltaTime);
        }
    }
}
