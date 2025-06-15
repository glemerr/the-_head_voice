using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BombEnemy : MonoBehaviour
{
    public Transform player;
    public GameObject bombPrefab;
    public Transform firePoint; // Attach this in the inspector (front of enemy)
    public float attackInterval = 5f;
    public float maxDistance = 15f;
    public float projectileSpeed = 20f;
    public float gravity = -10f;
    public float timeToLive = 5f;

    private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = attackInterval;
    }

    void Update()
    {
        if (!player) return;

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist > maxDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                ThrowBomb();
                timer = attackInterval;
            }
        }
    }

    void ThrowBomb()
    {
        Vector3 spawnPos = firePoint ? firePoint.position : transform.position + transform.forward * 1.5f;

        GameObject bomb = Instantiate(bombPrefab, spawnPos, Quaternion.identity);
        ParabolicGun parab = bomb.GetComponent<ParabolicGun>();

        if (parab != null)
        {
            Vector3 dir = (player.position - spawnPos + new Vector3(0, 2.5f, 0)).normalized; // Ignore vertical component for direction
            Vector3 initVel = dir * projectileSpeed;

            parab.initialVelocity = initVel;
            parab.gravity = gravity;
            parab.timeToLive = timeToLive;
        }
    }
}
