using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float delay = 2f;
    public float radius = 5f;
    public float force = 700f;
    public GameObject explosionEffect;

    void Start()
    {
        Invoke("Detonate", delay);
    }

    void Detonate()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearby in colliders)
        {
            Rigidbody rb = nearby.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }

            if (nearby.CompareTag("Player"))
            {
                // Player damage logic here
                Debug.Log("Player takes bomb damage!");
            }
        }

        Destroy(gameObject);
    }
}