using UnityEngine;
public class AutoDestroyParticles : MonoBehaviour
{
    private ParticleSystem ps;
    public float destroyDelay = 1.2f; // Default delay

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        //Destroy(gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        Destroy(gameObject, destroyDelay);
    }
}