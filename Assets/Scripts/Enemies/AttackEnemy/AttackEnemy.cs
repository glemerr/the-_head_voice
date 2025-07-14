using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AttackEnemy : Enemy
{
    public enum ChargeState { Chase, Charge }
    public new ChargeState currentState = ChargeState.Chase;

    [Header("Charge Settings")]
    public float chaseSpeed     = 3.5f;
    public float chargeSpeed    = 8f;
    public float chaseDuration  = 5f;

    [Header("Explosion Settings")]
    public GameObject explosionEffect;
    public float explosionRadius = 5f;
    public float explosionForce  = 700f;

    private float stateTimer;

    protected override void Start()
    {
        base.Start();
        agent.stoppingDistance = 0f;
        stateTimer = chaseDuration;
        currentState = ChargeState.Chase;
    }

    protected override void Update()
    {
        // count down timer
        stateTimer -= Time.deltaTime;

        switch (currentState)
        {
            case ChargeState.Chase:
                agent.speed = chaseSpeed;
                agent.isStopped = false;
                agent.SetDestination(player.position);

                if (stateTimer <= 0f)
                    EnterChargeState();
                break;

            case ChargeState.Charge:
                agent.speed = chargeSpeed;
                agent.isStopped = false;
                agent.SetDestination(player.position);
                break;
        }
    }

    private void EnterChargeState()
    {
        currentState = ChargeState.Charge;
        // reset timer if you want repeated cycles:
        // stateTimer = chaseDuration;
        // optional: play charge VFX or sound
    }

    // We don't use the base Attack() → DoAttack() here, so just stub it
    protected override void DoAttack() { }

    private void OnCollisionEnter(Collision collision)
    {
        // Only explode when charging and hitting the player
        if (currentState == ChargeState.Charge && collision.transform.CompareTag("Player"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        // Spawn VFX
        if (explosionEffect != null)
        {
            GameObject fx = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(fx, 2f);
        }

        // Apply force & damage in radius
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            // physics push
            if (hit.attachedRigidbody != null)
                hit.attachedRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);

            // damage the player
            if (hit.CompareTag("Player"))
            {
                // assume your PlayerHealth has TakeDamage(float)
                hit.GetComponent<LifeSystem>()?.TakeDamage(attackPower);
            }
        }

        lifeSystem?.TakeDamage(lifeSystem.Max); // self-destruct
        Destroy(gameObject); // destroy this enemy
    }
}
