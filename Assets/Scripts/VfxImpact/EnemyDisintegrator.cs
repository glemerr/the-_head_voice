using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(LifeSystem))]
public class EnemyDisintegrator : MonoBehaviour
{
    [Header("Dissolve Settings")]
    // Assign one or more dissolve materials in the Inspector
    [SerializeField] private Material[] dissolveMaterialTemplates;
    [SerializeField] private float dissolveDuration = 1.5f;
    
    [Header("Particle Effect")]
    [SerializeField] private GameObject disintegratePrefab;
    [SerializeField] private float particleLifetime = 2f;

    private bool isDisintegrating = false;
    private Renderer[] renderers;
    private Material[][] originalMaterials;
    private Material[] activeDissolveMaterials;

    private void OnEnable()
    {
        // Hook into the LifeSystem's OnDeath event
        GetComponent<LifeSystem>().OnDeath.AddListener(Disintegrate);
    }

    private void OnDisable()
    {
        GetComponent<LifeSystem>().OnDeath.RemoveListener(Disintegrate);
    }

    public void Disintegrate()
    {
        if (isDisintegrating) return;
        isDisintegrating = true;

        // 1. Turn off collisions
        if (TryGetComponent<Collider>(out Collider col))
            col.enabled = false;

        // 2. Cache original mats and swap in dissolve mats
        renderers = GetComponentsInChildren<Renderer>();
        originalMaterials = new Material[renderers.Length][];
        activeDissolveMaterials = new Material[renderers.Length];

        // Choose a random template or pick by index
        int templateIndex = Random.Range(0, dissolveMaterialTemplates.Length);
        Material chosenTemplate = dissolveMaterialTemplates[templateIndex];

        for (int i = 0; i < renderers.Length; i++)
        {
            // Save the original material array
            originalMaterials[i] = renderers[i].materials;

            // Instantiate a unique dissolve material instance
            Material inst = Instantiate(chosenTemplate);
            activeDissolveMaterials[i] = inst;

            // If the renderer has multiple slots, fill all slots
            var mats = new Material[renderers[i].materials.Length];
            for (int j = 0; j < mats.Length; j++)
                mats[j] = inst;

            renderers[i].materials = mats;
        }

        // 3. Spawn particle effect if assigned
        if (disintegratePrefab != null)
        {
            var particles = Instantiate(
                disintegratePrefab,
                transform.position,
                Quaternion.identity
            );
            Destroy(particles, particleLifetime);
        }

        // 4. Animate dissolve over time, then destroy
        StartCoroutine(DoDissolve());
    }

    private IEnumerator DoDissolve()
    {
        float elapsed = 0f;
        while (elapsed < dissolveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / dissolveDuration);

            // Update dissolve parameter on each material
            foreach (var mat in activeDissolveMaterials)
                mat.SetFloat("_DissolveAmount", t);

            yield return null;
        }

        // Ensure fully dissolved
        foreach (var mat in activeDissolveMaterials)
            mat.SetFloat("_DissolveAmount", 1f);

        Destroy(gameObject);
    }

    // Call this if you reuse enemies via pooling
    public void RestoreOriginalState()
    {
        if (renderers == null || originalMaterials == null) return;

        for (int i = 0; i < renderers.Length; i++)
            renderers[i].materials = originalMaterials[i];

        isDisintegrating = false;
    }
}