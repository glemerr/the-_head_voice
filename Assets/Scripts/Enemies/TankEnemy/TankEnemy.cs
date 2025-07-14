using UnityEngine;

public class TankEnemy : Enemy
{
    [Header("Tank Settings")]
    public float shieldStrength = 50f;
    public float shieldRegenRate = 5f;

    private float currentShield;

    protected override void Start()
    {
        base.Start();
        currentShield = shieldStrength;
    }

    public void TakeDamage(float amount)
    {
        // absorb with shield first
        float leftover = amount - currentShield;
        currentShield = Mathf.Max(0, currentShield - amount);
        if (leftover > 0) lifeSystem.TakeDamage(leftover);
    }

    protected override void Update()
    {
        base.Update();
        // Regenerate shield when not in combat
        if (currentState == State.Patrol || currentState == State.Flee)
            currentShield = Mathf.Min(shieldStrength, currentShield + shieldRegenRate * Time.deltaTime);
    }

    protected override void DoAttack()
    {
        // 1) Play attack animation here...
        // 2) Then do a physics check:
        Vector3 center = transform.position + transform.forward * attackRange * 0.5f;
        float radius = attackRange * 0.5f;

        Collider[] hits = Physics.OverlapSphere(center, radius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                LifeSystem playerLife = hit.GetComponent<LifeSystem>();
                if (playerLife != null)
                    playerLife.TakeDamage(attackPower * 1.5f);
            }
        }
    }

}
