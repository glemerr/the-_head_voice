using UnityEngine;

public class SniperBullet : Bullet
{
    public GameObject owner;  // Reference to the shooter

    protected override void OnHit(Collider hit)
    {
        // Ignore collisions with owner and non-player objects
        if (hit.gameObject == owner || !hit.CompareTag("Player")) 
            return;

        var lifeSystem = hit.GetComponent<LifeSystem>();
        if (lifeSystem != null)
        {
            lifeSystem.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}