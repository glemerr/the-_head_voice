using UnityEngine;

public class FlameBullet : Bullet
{
    [Header("Flame Settings")]
    public float burnDuration = 3f;
    public float burnTickRate = 1f;
    public float burnDamagePerTick = 2f;

    protected override void OnHit(Collider hit)
    {
        var enemy = hit.GetComponent<LifeSystem>();
        if (enemy != null)
        {
            // initial hit
            enemy.TakeDamage(damage);
            // then apply DOT
            enemy.StartCoroutine(Ignite(enemy));
        }

        // optional: spawn fire decal or VFX
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
