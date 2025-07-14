using UnityEngine;

public class BazookaShell : Bullet
{
    [Header("Explosive Settings")]
    public float blastRadius = 5f;
    public float blastForce  = 700f;
    [Header("Impact Effects")]
    public ImpactEffect enemyImpactEffect;
    public ImpactEffect defaultImpactEffect;
    
    protected override void OnHit(Collider hit)
    {
        // spawn explosion VFX
        // e.g. Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Collider[] hits = Physics.OverlapSphere(transform.position, blastRadius);
        foreach (var col in hits)
        {
            // physics knockback
            var body = col.attachedRigidbody;
            if (body != null)
                body.AddExplosionForce(blastForce, transform.position, blastRadius);

            // apply damage to enemies
            var enemy = hit.GetComponent<LifeSystem>();

            // Apply damage if it's an enemy
            if (enemy != null && hit.CompareTag("Enemy"))
            {
                enemy.TakeDamage(damage);
                // Play enemy-specific impact effect
                enemyImpactEffect.PlayEffect(transform.position, hit.transform);
            }
            else
            {
                // Play default impact effect for non-enemies
                defaultImpactEffect.PlayEffect(transform.position);
            }

            // Destroy the bullet
            Destroy(gameObject, lifetime);
        }
    }
}
