using UnityEngine;

public class BazookaShell : Bullet
{
    [Header("Explosive Settings")]
    public float blastRadius = 5f;
    public float blastForce  = 700f;

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
            var enemy = col.GetComponent<LifeSystem>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
