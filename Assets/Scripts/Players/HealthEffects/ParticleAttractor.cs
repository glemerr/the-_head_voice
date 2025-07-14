using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleAttractor : MonoBehaviour
{
    [Header("Attraction Settings")]
    [Tooltip("What to pull particles toward")]
    public GameObject target;
    [Tooltip("Max distance at which attraction begins")]
    public float attractRadius = 5f;
    [Tooltip("How strong the pull is (units/sec²)")]
    public float strength = 10f;

    [Header("Healing Settings")]
    [Tooltip("Health to restore per particle")]
    public float healPerParticle = 2f;

    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;
    private LifeSystem lifeSystem;
    private List<ParticleSystem.Particle> enterList = new List<ParticleSystem.Particle>();

    [ SerializeField] private DamageEffects damageEffects;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();

        // Make sure your PS is in World space:
        var main = ps.main;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
    }

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("ParticleAttractor: No target assigned!", this);
            enabled = false;
            return;
        }

        // Cache player components
        lifeSystem = target.GetComponent<LifeSystem>();
        var col = target.GetComponent<Collider>();
        if (col == null)
            Debug.LogWarning("ParticleAttractor: target has no Collider!", this);

        // Configure Trigger module
        var trigger = ps.trigger;
        trigger.enabled = true;
        if (col != null)
            trigger.SetCollider(0, col);
        // Kill on enter so engine removes them automatically
        trigger.enter = ParticleSystemOverlapAction.Kill;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Ensure our array can hold all particles
        int max = ps.main.maxParticles;
        if (particles == null || particles.Length < max)
            particles = new ParticleSystem.Particle[max];

        int count = ps.GetParticles(particles);
        Vector3 playerPos = target.transform.position;

        // Attract each particle
        for (int i = 0; i < count; i++)
        {
            Vector3 worldPos = particles[i].position;
            Vector3 toPlayer = playerPos - worldPos;
            float dist = toPlayer.magnitude;

            if (dist < attractRadius && dist > 0.001f)
            {
                Vector3 dir = toPlayer / dist;
                particles[i].velocity += dir * strength * Time.deltaTime;
            }
            if (dist < 2f)
            {
                // If too close, snap to player position
                //Debug.Log($"Particle {i} too close, Healing and snapping to player");
                lifeSystem?.Heal(healPerParticle); // Heal player
                lifeSystem.Add(healPerParticle); // Add to life system
                damageEffects?.ShowDamageEffects(); // Show damage effects
                // damageEffects.damageOverlay.color = new Color(0, 1, 0, 0.8f);
                // var mainModule = damageEffects.bloodEffect.main;
                // mainModule.startColor = new Color(0, 1, 0, 0.8f);
            }
        }

        // Commit changes
        ps.SetParticles(particles, count);
    }

    void OnParticleTrigger()
    {
        if (lifeSystem == null) return;

        // Grab particles that just entered our trigger
        enterList.Clear();
        int numEntered = ps.GetTriggerParticles(
            ParticleSystemTriggerEventType.Enter,
            enterList
        );

        if (numEntered > 0)
        {
            // Heal player: +healPerParticle per killed particle
            float totalHeal = numEntered * healPerParticle;
            lifeSystem.Heal(totalHeal);

            // (Optional) Debug
            Debug.Log($"Ate {numEntered} particles, healed {totalHeal} HP");
        }
    }
}