using UnityEngine;

public class QuickQuicksandEffect : MonoBehaviour
{
    [Header("Lake Damage Settings")]
    [SerializeField] private float damagePerSecond = 5f;
    
    [Header("Quicksand Sinking Settings")]
    [SerializeField] private float sinkSpeed = 0.5f; // How fast the object sinks (units per second)
    [SerializeField] private bool applySinking = true;

    private float damageTimer = 0f;

    private void OnTriggerStay(Collider other)
    {
        // Check if the colliding object is the Player or an Enemy
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            // Damage application over time
            damageTimer += Time.deltaTime;
            if (damageTimer >= 1f)
            {
                LifeSystem life = other.GetComponent<LifeSystem>();
                if (life != null)
                {
                    life.TakeDamage(damagePerSecond);
                }
                damageTimer = 0f;
            }

            // Sinking effect to simulate being swallowed by quicksand
            if (applySinking)
            {
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // If using physics, add a constant downward acceleration
                    Vector3 downwardForce = new Vector3(0f, -sinkSpeed, 0f);
                    rb.AddForce(downwardForce, ForceMode.Acceleration);
                }
                else
                {
                    // If no Rigidbody is present, modify the transform position
                    other.transform.position += Vector3.down * sinkSpeed * Time.deltaTime;
                }
            }
        }
    }
}