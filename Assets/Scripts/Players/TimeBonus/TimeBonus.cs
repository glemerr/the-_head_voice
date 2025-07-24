using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class TimeBonus : MonoBehaviour
{
    [Header("Attraction Settings")]
    [Tooltip("What to pull particles toward")]
    public GameObject target;
    [Tooltip("Max distance at which attraction begins")]
    public float attractRadius = 5f;
    [Tooltip("How strong the pull is (units/sec²)")]
    public float strength = 10f;

    [Header("Time Settings")]
    [Tooltip("Seconds to add per particle")]
    public float timePerParticle = 1f;

    [Header("Effects")]
    [SerializeField] private DamageEffects damageEffects;

    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;
    private List<ParticleSystem.Particle> enterList = new List<ParticleSystem.Particle>();

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        // Ensure world‐space simulation so positions & triggers line up
        var main = ps.main;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
    }

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("TimeBonus: No target assigned!", this);
            enabled = false;
            return;
        }

        // Add the target's collider to the trigger module
        var col = target.GetComponent<Collider>();
        var trigger = ps.trigger;
        trigger.enabled = true;
        if (col != null)
            trigger.SetCollider(0, col);
        trigger.enter = ParticleSystemOverlapAction.Kill;
    }

    void LateUpdate()
    {
        if (target == null) return;

        int maxParticles = ps.main.maxParticles;
        if (particles == null || particles.Length < maxParticles)
            particles = new ParticleSystem.Particle[maxParticles];

        int count = ps.GetParticles(particles);
        Vector3 playerPos = target.transform.position;

        for (int i = 0; i < count; i++)
        {
            Vector3 worldPos = particles[i].position;
            Vector3 toTarget = playerPos - worldPos;
            float dist = toTarget.magnitude;

            if (dist < attractRadius && dist > 0.001f)
            {
                Vector3 dir = toTarget / dist;
                particles[i].velocity += dir * strength * Time.deltaTime;
            }
        }

        ps.SetParticles(particles, count);
    }

    void OnParticleTrigger()
    {
        Debug.Log("TimeBonus: OnParticleTrigger called");
        if (ZoneManager.Instance == null)
        {
            Debug.LogError("TimeBonus: ZoneManager instance not found!", this);
            return;
        }    
    

        enterList.Clear();
        int numEntered = ps.GetTriggerParticles(
            ParticleSystemTriggerEventType.Enter,
            enterList
        );

        if (numEntered > 0)
        {
            float totalTime = numEntered * timePerParticle;
            ZoneManager.Instance.AddTime(totalTime);
            damageEffects?.ShowDamageEffects();

            Debug.Log($"TimeBonus: Consumed {numEntered} particles, added {totalTime} seconds.");
            // NotificationManager.Instance.ShowMissionNotification(
            //     "TimeBonus:TimeBonus:",
            //     $"TimeBonus: Consumed {numEntered} particles, added {totalTime} seconds."
            // );
        }
    }
}