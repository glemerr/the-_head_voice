using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(LifeSystem))]
public class EnemyDisintegrator : MonoBehaviour
{
    [Header("Dissolve Settings")]
    [SerializeField] private Material dissolveMaterialTemplate;
    [SerializeField] private float dissolveDuration = 1.5f;

    [Header("Particle Effect")]
    [SerializeField] private GameObject disintegratePrefab;
    [SerializeField] private float particleLifetime = 2f;

    private bool isDisintegrating = false;
    private Renderer[] renderers;
    private Material[][] originalMats;
    private Material[] dissolveInstances;

    private void OnEnable()
    {
        // Subscribe to the enemy’s death event
        GetComponent<LifeSystem>().OnDeath.AddListener(Disintegrate);
    }

    private void OnDisable()
    {
        GetComponent<LifeSystem>().OnDeath.RemoveListener(Disintegrate);
    }

    public void Disintegrate()
    {
        if (isDisintegrating) 
            return;

        isDisintegrating = true;

        // 1. Disable physics and collisions
        if (TryGetComponent<Collider>(out Collider col))
            col.enabled = false;

        // 2. Cache original materials & swap to dissolve material per renderer
        renderers = GetComponentsInChildren<Renderer>();
        originalMats = new Material[renderers.Length][];
        dissolveInstances = new Material[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            // Store each renderer’s full material array
            originalMats[i] = renderers[i].materials;

            // Instantiate a fresh dissolve material for this renderer
            var inst = Instantiate(dissolveMaterialTemplate);
            dissolveInstances[i] = inst;

            // Apply it (assuming single‐material shaders; for multiple mats, assign an array)
            renderers[i].material = inst;
        }

        // 3. Spawn and auto‐destroy particle effect
        if (disintegratePrefab != null)
        {
            var particles = Instantiate(disintegratePrefab, transform.position, Quaternion.identity);
            Destroy(particles, particleLifetime);
        }

        // 4. Animate dissolve then destroy
        StartCoroutine(DoDissolve());
    }

    private IEnumerator DoDissolve()
    {
        float elapsed = 0f;
        while (elapsed < dissolveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / dissolveDuration);

            // Update each material’s dissolve parameter
            foreach (var mat in dissolveInstances)
                mat.SetFloat("_DissolveAmount", t);

            yield return null;
        }

        // Ensure fully dissolved
        foreach (var mat in dissolveInstances)
            mat.SetFloat("_DissolveAmount", 1f);

        Destroy(gameObject);
    }

    // Optional: if you pool enemies instead of destroying
    public void RestoreOriginalState()
    {
        if (renderers == null || originalMats == null)
            return;

        for (int i = 0; i < renderers.Length; i++)
            renderers[i].materials = originalMats[i];

        isDisintegrating = false;
    }
}