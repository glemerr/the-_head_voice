using UnityEngine;

public class ShotgunPellet : Bullet
{
    protected override void OnHit(Collider hit)
    {
        var enemy = hit.GetComponent<LifeSystem>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        // optional: spawn spark VFX at hit.point
    }
}