using System.Collections;
using UnityEngine;

public class EnemyDisintegrator : MonoBehaviour
{
    [Header("Dissolve Settings")]
    public Material dissolveMaterial; // M_Dissolve
    public float dissolveDuration = 1.5f;

    [Header("Particle Effect")]
    public GameObject disintegratePrefab; // PF_Disintegrate

    private Material[] originalMats;
    private Renderer[] rends;

    public void Disintegrate()
    {
        // 1. Cache original materials
        rends = GetComponentsInChildren<Renderer>();
        originalMats = new Material[rends.Length];
        for (int i = 0; i < rends.Length; i++)
        {
            originalMats[i] = rends[i].material;
            rends[i].material = dissolveMaterial;
        }

        // 2. Spawn particles
        GameObject particules = Instantiate(disintegratePrefab, transform.position, Quaternion.identity);

        // 3. Animate dissolve
        StartCoroutine(DoDissolve());
        Destroy(particules, 2f); // Destroy particles after 2 seconds
    }

    private IEnumerator DoDissolve()
    {
        float elapsed = 0f;
        // assume dissolveMaterial is an instance
        while (elapsed < dissolveDuration)
        {
            float t = elapsed / dissolveDuration;
            dissolveMaterial.SetFloat("_DissolveAmount", t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        dissolveMaterial.SetFloat("_DissolveAmount", 1f);

        // 4. Cleanup
        //Destroy(gameObject);
    }
}
