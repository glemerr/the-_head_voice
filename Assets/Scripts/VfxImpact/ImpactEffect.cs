using UnityEngine;

[System.Serializable]
public class ImpactEffect
{
    public GameObject hitParticlePrefab;
    public AudioClip hitSound;
    [Range(0, 1)] public float hitSoundVolume = 0.7f;
    
    public void PlayEffect(Vector3 position, Transform parent = null)
    {
        if (hitParticlePrefab != null)
        {
            GameObject particles = Object.Instantiate(
                hitParticlePrefab, 
                position, 
                Quaternion.identity,
                parent);
            
            // Auto-destroy the particle system after it plays
            ParticleSystem ps = particles.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Object.Destroy(particles, ps.main.duration);
            }
            else
            {
                Object.Destroy(particles, 2f);
            }
        }
        
        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, position, hitSoundVolume);
        }
    }
}