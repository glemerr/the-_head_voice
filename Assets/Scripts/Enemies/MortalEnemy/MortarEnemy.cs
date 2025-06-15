using UnityEngine;

public class MortarEnemy : Enemy
{
    [Header("Mortar Settings")]
    public GameObject shellPrefab;
    public Transform firePoint;
    public float launchForce = 12f;
    public float arcHeight = 5f;
    public float splashRadius = 3f;

    protected override void DoAttack()
    {
        if (shellPrefab == null || firePoint == null) return;

        // Calculate ballistic arc to hit player's current position
        Vector3 targetPos = player.position + Vector3.up * 1.5f;
        Vector3 delta = targetPos - firePoint.position;
        Vector3 velocity = delta / attackCooldown; 
        velocity.y += arcHeight; 

        GameObject shell = Instantiate(shellPrefab, firePoint.position, Quaternion.identity);
        if (shell.TryGetComponent<Rigidbody>(out var rb))
            rb.linearVelocity = velocity;

        // On impact, your shell’s script should do:
        // Collider[] hits = Physics.OverlapSphere(transform.position, splashRadius);
        // foreach (var c in hits) c.GetComponent<Enemy>()?.TakeDamage(attackPower);
    }
}
