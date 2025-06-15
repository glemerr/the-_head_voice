using UnityEngine;
using System.Collections;

public class Portal : Bullet
{
    [Header("Portal Settings")]
    public float minHeightY = 7f;       // The minimum Y position allowed.
    public float playerOffset = 2f;     // Offset above the player's Y position.
    
    protected override void Start()
    {
        AdjustPositionRelativeToPlayer();
    }

    void Update()
    {
        // Enforce the minimum height every frame.
        if (transform.position.y < minHeightY)
        {
            transform.position = new Vector3(transform.position.x, minHeightY, transform.position.z);
        }
    }
    
    // Adjust the portal's vertical position based on the player's height plus an offset.
    void AdjustPositionRelativeToPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            
            float desiredY = player.transform.position.y + playerOffset;
            // Clamp to the minimum allowed height.
            if (desiredY < minHeightY)
            {
                desiredY = minHeightY;
            }
            transform.position = new Vector3(transform.position.x, desiredY, transform.position.z);
        }
        else
        {
            Debug.LogWarning("Player not found! Ensure the player GameObject is tagged as \"Player\".");
        }
    }

    protected override void OnHit(Collider other)
    {
        // Your hit logic here.
        Debug.Log("Portal OnHit called with: " + other.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            return;

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Portal triggered by Enemy");
            // Start the absorption animation before destroying the enemy.
            StartCoroutine(AbsorbEnemy(other.gameObject));
        }
    }

    // Coroutine that animates the enemy's absorption.
    private IEnumerator AbsorbEnemy(GameObject enemy)
    {
        // Cache the enemy's transform reference.
        Transform enemyT = enemy.transform;
        Vector3 startPosition = enemyT.position;
        Vector3 startScale = enemyT.localScale;
        Vector3 targetPosition = transform.position;
        float duration = 0.8f; // Total duration of the effect.
        float elapsed = 0.2f;

        while (elapsed < duration)
        {
            // Check if the enemy is still valid.
            if (enemy == null)
                yield break;

            float t = elapsed / duration;

            // Base interpolation toward the portal center.
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, t);

            // Add a spiral effect.
            float spiralRadius = Mathf.Lerp(1f, 0f, t); // Spiral radius shrinks over time.
            float angle = t * Mathf.PI * 4f;            // Several rotations.
            float offsetX = Mathf.Cos(angle) * spiralRadius;
            float offsetZ = Mathf.Sin(angle) * spiralRadius;
            float offsetY = Mathf.Sin(angle * 0.5f) * spiralRadius * 0.2f;

            enemyT.position = newPosition + new Vector3(offsetX, offsetY, offsetZ);

            // Gradually decrease the enemy's scale.
            enemyT.localScale = Vector3.Lerp(startScale, Vector3.zero, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // After the animation, ensure the enemy still exists before destroying.
        if (enemy != null)
        {
            Destroy(enemy);
        }
    }
}