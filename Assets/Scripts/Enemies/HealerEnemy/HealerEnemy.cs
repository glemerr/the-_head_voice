using UnityEngine;
using System.Linq;

public class HealerEnemy : Enemy
{
    [Header("Healer Settings")]
    public float healAmount = 15f;
    public float healRange = 8f;
    public float healCooldown = 5f;

    private float lastHealTime;

    protected override void DoAttack()
    {
        // Instead of damaging the player, heal the weakest ally in range
    //     if (Time.time < lastHealTime + healCooldown) return;
    //     lastHealTime = Time.time;

    //     // Find all other Enemy instances within healRange
    //     var allies = FindObjectsByType<Enemy>(FindObjectsSortMode.None)
    //         .Where(e => e != this && Vector3.Distance(e.transform.position, transform.position) <= healRange)
    //         .OrderBy(e => e.currentHealth)
    //         .ToList();

    //     if (allies.Count > 0)
    //     {
    //         var weakest = allies.First();
    //         weakest.currentHealth = Mathf.Min(weakest.maxHealth, weakest.currentHealth + healAmount);
    //         Debug.Log($"{name} heals {weakest.name} for {healAmount} HP!");
    //     }
    // }

    // protected override void Update()
    // {
    //     // Healer doesn’t chase the player—patrols and heals allies
    //     if (Time.time >= lastHealTime + healCooldown)
    //     {
    //         TransitionTo(State.Patrol);
    //         Patrol();
    //     }
    //     else base.Update();
     }
}   
