using UnityEngine;

public class FlameBullet : Bullet
{
    [Header("Flame Settings")]
    public float burnDuration = 3f;
    public float burnTickRate = 1f;
    public float burnDamagePerTick = 2f;
    
        [Header("Impact Effects")]
        public ImpactEffect enemyImpactEffect;
        public ImpactEffect defaultImpactEffect;

    protected override void OnHit(Collider hit)
    {
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

    private System.Collections.IEnumerator Ignite(LifeSystem target)
    {
        float elapsed = 0f;
        while (elapsed < burnDuration)
        {
            yield return new WaitForSeconds(burnTickRate);
            target.TakeDamage(burnDamagePerTick);
            elapsed += burnTickRate;
        }
    }
}
