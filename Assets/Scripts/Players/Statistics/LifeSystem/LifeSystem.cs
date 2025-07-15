using UnityEngine;
using UnityEngine.Events;

public class LifeSystem : Statistics
{
    [Header("Health Events")]
    public UnityEvent OnDeath;

    // Prevents multiple death triggers
    private bool isDead = false;

    public virtual void TakeDamage(float damage)
    {
        if (damage < 0 || isDead) 
            return;

        Subtract(damage);

        if (Current <= Min)
        {
            isDead = true;
            OnDeath?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        if (amount < 0 || isDead) 
            return;

        Add(amount);
    }

    public bool CanTakeDamage(float amount)
    {
        if (isDead || Current < amount)
            return false;

        Subtract(amount);
        return true;
    }
}