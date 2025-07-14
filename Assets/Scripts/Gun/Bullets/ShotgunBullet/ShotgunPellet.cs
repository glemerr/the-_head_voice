using UnityEngine;

public class ShotgunPellet : Bullet
{
    [Header("Impact Effects")]
    public ImpactEffect enemyImpactEffect;
    public ImpactEffect defaultImpactEffect;
    
    protected override void OnHit(Collider hit)
    {
        // Try to get the LifeSystem component
        var enemy = hit.GetComponent<LifeSystem>();
        
        // Apply damage if it's an enemy
        if (enemy != null)
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
        Destroy(gameObject,lifetime);
    }
}