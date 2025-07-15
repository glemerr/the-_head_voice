using UnityEngine;

public class CollectablePickup : MonoBehaviour
{
    [Header("Collectable Settings")]
    [SerializeField] private CollectableObject collectableData;
    [SerializeField] private int quantity = 1;
    [SerializeField] private float pickupRadius = 1.5f;
    [SerializeField] private GameObject pickupEffect;

    void Start()
    {
        // Auto-add sphere collider for detection
        SphereCollider collider = gameObject.AddComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = pickupRadius;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AttemptPickup();
        }
    }

    public void AttemptPickup()
    {
        if (InventoryManager.Instance.AddCollectable(collectableData, quantity))
        {
            // Successfully picked up
            if (pickupEffect) Instantiate(pickupEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    // Visual helper in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}