using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserRay : Bullet
{
    [Header("Laser Settings")]
    public float laserLength = 100f;
    private float laserDuration;
    public LayerMask hitMask;

    private LineRenderer lineRenderer;
    private Camera playerCamera;

    protected override void Start()
    {
        base.Start();
        rb.linearVelocity = Vector3.zero; // Laser doesn't move physically

        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("LaserRay requires a LineRenderer child.");
        }
        playerCamera = Camera.main;

        laserDuration = lifetime;
        Destroy(gameObject, laserDuration);
    }

    void Update()
    {
        if (!playerCamera) return;

        // Sync position and rotation with camera
        transform.position = playerCamera.transform.position + playerCamera.transform.forward  + new Vector3(0f, -0.7f, 0f);
        transform.rotation = Quaternion.LookRotation(playerCamera.transform.forward);

        // Recalculate and draw laser beam
        FireLaser();
    }

    void FireLaser()
    {
        Vector3 startPosition = transform.position; 
        Vector3 direction = playerCamera.transform.forward;
        Vector3 endPosition = startPosition + direction * laserLength;

        Ray ray = new Ray(startPosition, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, laserLength))
        {
            endPosition = hit.point;

            if (hit.collider.CompareTag("Enemy"))
            {
                LifeSystem life = hit.collider.GetComponent<LifeSystem>();
                if (life != null)
                {
                    life.TakeDamage(damage);
                }
            }
        }

        // Draw LineRenderer from camera forward
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);
    }

    protected override void OnHit(Collider hit)
    {
        // Not used — laser damage is applied via ray
    }
}
