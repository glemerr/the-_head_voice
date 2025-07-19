using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BossLaser : MonoBehaviour
{
    [Header("Laser Settings")]
    public float damagePerSecond = 5f;
    public Transform firePoint;
    public float maxLength = 10f;
    public LayerMask hitMask;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (firePoint == null)
        {
            Debug.LogError($"{gameObject.name} no tiene asignado un firePoint.");
        }
    }

    private void Update()
    {
        if (firePoint == null || lineRenderer == null) return;

        Vector3 origin = firePoint.position;
        Vector3 direction = firePoint.forward;
        Vector3 endPoint = origin + direction * maxLength;

        // Raycast para detectar colisión
        if (Physics.Raycast(origin, direction, out RaycastHit hit, maxLength, hitMask))
        {
            endPoint = hit.point;

            // Si golpea al jugador, causa daño
            if (hit.collider.CompareTag("Player"))
            {
                LifeSystem life = hit.collider.GetComponent<LifeSystem>();
                if (life != null)
                {
                    life.TakeDamage(damagePerSecond * Time.deltaTime);
                }
            }
        }

        // Dibujar el rayo
        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, endPoint);
    }
}
