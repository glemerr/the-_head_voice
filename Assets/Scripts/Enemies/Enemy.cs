using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public abstract class Enemy : MonoBehaviour
{
    public enum State { Patrol, Chase, Attack, Flee }

    [Header("Stats")]
    public float maxHealth = 100f;
    public float attackPower = 10f;
    public float defense = 5f;

    [Header("AI Settings")]
    public float patrolRadius = 10f;
    public float chaseRange = 15f;
    public float attackRange = 3f;
    public float fleeHealthThreshold = 20f;
    public float attackCooldown = 1.5f;

    protected State currentState;
    protected NavMeshAgent agent;
    protected Transform player;
    protected float currentHealth;
    protected float lastAttackTime;
    protected Vector3 patrolCenter;
    protected Vector3 patrolTarget;

    protected LifeSystem lifeSystem;

    [SerializeField] private UIenemyHealth healthUI;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        lifeSystem = GetComponent<LifeSystem>();
        currentHealth = lifeSystem.Max;
        patrolCenter = transform.position;
        ChooseNewPatrolPoint();
        TransitionTo(State.Patrol);
        healthUI.UpdateHealth(lifeSystem.Current, lifeSystem.Max);
        lifeSystem.OnDeath.AddListener(() => StartCoroutine(DestroyAfterDeathAnimation()));
    }

    protected virtual void Update()
    {
        if (player == null) return;

        // 👇 Verificar invisibilidad del jugador
        FirstPersonController controller = player.GetComponent<FirstPersonController>();
        if (controller != null && controller.isInvisible)
        {
            agent.ResetPath(); // opcional: detener movimiento
            return;
        }

        float dist = Vector3.Distance(transform.position, player.position);
        switch (currentState)
        {
            case State.Patrol:
                if (dist < chaseRange) TransitionTo(State.Chase);
                Patrol();
                break;

            case State.Chase:
                if (currentHealth < fleeHealthThreshold) TransitionTo(State.Flee);
                else if (dist <= attackRange) TransitionTo(State.Attack);
                else if (dist > chaseRange * 1.2f) TransitionTo(State.Patrol);
                Chase();
                break;

            case State.Attack:
                if (currentHealth < fleeHealthThreshold) TransitionTo(State.Flee);
                else if (dist > attackRange) TransitionTo(State.Chase);
                Attack();
                break;

            case State.Flee:
                Flee();
                if (currentHealth > maxHealth * 0.5f && dist > chaseRange)
                    TransitionTo(State.Patrol);
                break;
        }

        healthUI.UpdateHealth(lifeSystem.Current, lifeSystem.Max);
    }

    protected void TransitionTo(State newState)
    {
        currentState = newState;
        // Puedes agregar lógica extra al cambiar de estado si lo deseas
    }

    #region Behaviors
    protected virtual void Patrol()
    {
        if (agent.remainingDistance < 0.5f)
            ChooseNewPatrolPoint();

        agent.SetDestination(patrolTarget);
    }

    void ChooseNewPatrolPoint()
    {
        Vector2 rnd = Random.insideUnitCircle * patrolRadius;
        patrolTarget = patrolCenter + new Vector3(rnd.x, 0, rnd.y);
    }

    protected virtual void Chase()
    {
        agent.SetDestination(player.position);
    }

    protected virtual void Attack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;
        lastAttackTime = Time.time;
        DoAttack();
    }

    protected abstract void DoAttack();

    protected virtual void Flee()
    {
        Vector3 dirAway = (transform.position - player.position).normalized;
        Vector3 fleeTarget = transform.position + dirAway * patrolRadius;
        agent.SetDestination(fleeTarget);
    }
    #endregion

    private IEnumerator DestroyAfterDeathAnimation()
    {
        float deathAnimLength = 1.5f; // Ajusta según la duración de la animación
        yield return new WaitForSeconds(deathAnimLength);

        ItemManager manager = FindFirstObjectByType<ItemManager>();
        if (manager != null)
        {
            manager.TrySpawnRandomItem(transform.position);
        }

        Destroy(gameObject);
    }
}
