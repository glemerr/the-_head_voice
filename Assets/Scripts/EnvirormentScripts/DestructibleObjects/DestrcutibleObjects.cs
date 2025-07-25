using UnityEngine;
using System.Collections;

public class DestructibleObject : MonoBehaviour
{
    [Header("VFX Settings")]
    public GameObject destructionVFXPrefab; // Assign in inspector
    public float vfxDuration = 2f; // How long VFX lasts before auto-destroy
    
    [Header("Death Animation")]
    public float deathAnimLength = 0.5f;
    
    private LifeSystem objectLife;
    private bool isDying = false;

    private void Start()
    {
        objectLife = GetComponent<LifeSystem>();
        if (objectLife == null)
        {
            Debug.LogError("DestructibleObject script requires a LifeSystem component on the same GameObject.");
            return;
        }
        objectLife.OnDeath.AddListener(StartDestructionSequence);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision detected with: " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Bullet"))
        {
            // Better approach: Damage player instead of destroying
            //LifeSystem objectLife = GetComponent<LifeSystem>();
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            //bullet.damage;
            if (objectLife != null && bullet != null)
            {
                objectLife.TakeDamage(bullet.damage); // Adjust damage as needed
            }
            else
            {
                //Debug.LogWarning("Bullet or LifeSystem component not found on collision object.");
                objectLife.TakeDamage(10);
            }
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger detected with: " + other.gameObject.tag);
        if (other.CompareTag("Player") || other.CompareTag("Bullet"))
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            //Debug.Log("Bullet detected: " + (bullet != null ? bullet.damage : "null"));
            //bullet.damage;
            if (objectLife != null && bullet != null)
            {
                objectLife.TakeDamage(bullet.damage); // Adjust damage as needed
            }
            else
            {
                //Debug.LogWarning("Bullet or LifeSystem component not found on collision object.");
                objectLife.TakeDamage(10);
            }

        }
    }
    private void StartDestructionSequence()
    {
        if (isDying) return;
        isDying = true;
        
        StartCoroutine(DestroyAfterDeathAnimation());
    }

    private IEnumerator DestroyAfterDeathAnimation()
    {
        // Disable colliders and renderers
        DisableComponents();
        
        // Wait for death animation
        yield return new WaitForSeconds(deathAnimLength);
        
        // Spawn item if available
        TrySpawnPowerUp();
        
        // Play destruction VFX
        PlayDestructionVFX();
        AudioManager.Instance.PlayExplosionSound();
        // Finally destroy the object
        Destroy(gameObject);
    }

    private void DisableComponents()
    {
        // Disable all colliders
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
        
        // Disable all renderers
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            rend.enabled = false;
        }
        
        // Disable other components if needed
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;
    }

    private void  TrySpawnPowerUp()
    {
        PowerUpsManager  manager = FindFirstObjectByType<PowerUpsManager>();
        Debug.Assert(manager != null, "PowerUpsManager not found in the scene. Please ensure it is present.");
        if (manager != null)
        {
            manager.TrySpawnRandomPowerUp(transform.position);
        }
    }

    private void PlayDestructionVFX()
    {
        if (destructionVFXPrefab != null)
        {
            // Instantiate VFX at object's position
            GameObject vfxInstance = Instantiate(
                destructionVFXPrefab, 
                transform.position, 
                transform.rotation
            );
            
            // Auto-destroy VFX after duration
            Destroy(vfxInstance, vfxDuration);
        }
        else
        {
            Debug.LogWarning("No destruction VFX prefab assigned to " + gameObject.name);
        }
    }
}