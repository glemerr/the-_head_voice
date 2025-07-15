using UnityEngine;

public class EnemyBombBullet : Bullet

{
    [Header("Impact Effects")]
    public ImpactEffect enemyImpactEffect;
    public ImpactEffect defaultImpactEffect;
    [SerializeField][Range(0.1f, 10f)] private float destroyTimer = 1f;
    public GameObject owner; 

    protected override void OnHit(Collider hit)
    {
        // Ignore collisions with owner and non-player objects
        if (hit.gameObject == owner || !hit.CompareTag("Player")) 
            return;

        var lifeSystem = hit.GetComponent<LifeSystem>();
        if (lifeSystem != null)
        {
            lifeSystem.TakeDamage(damage);
            enemyImpactEffect.PlayEffect(transform.position, hit.transform);
        }
        else
        {
            defaultImpactEffect.PlayEffect(transform.position);
        }

        Destroy(gameObject);
    }
}
