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
        // simple heavy melee
        Debug.Log($"{name} slams for {attackPower * 1.5f} damage!");
        // e.g. player.GetComponent<PlayerHealth>().TakeDamage(attackPower * 1.5f);
    }
}
