using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public abstract class Bullet : MonoBehaviour
{
    [Header("Core Settings")]
    public float speed      = 20f;
    public float damage     = 10f;
    public float lifetime   = 5f;
    public Vector3 direction;

    protected Rigidbody rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // make sure collider is trigger for instant callbacks
        GetComponent<Collider>().isTrigger = true;
    }

    protected virtual void Start()
    {
        Destroy(gameObject, lifetime);
        rb.linearVelocity = direction.normalized * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        // ignore if hitting another bullet or trigger zone
        if (other.isTrigger) return;

        OnHit(other);
        //Destroy(gameObject);
    }

    /// <summary>
    /// Called when this bullet hits something non-trigger.
    /// </summary>
    protected abstract void OnHit(Collider hit);
}
