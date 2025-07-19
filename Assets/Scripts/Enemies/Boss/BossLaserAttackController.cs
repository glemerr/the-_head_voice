using UnityEngine;

public class BossLaserAttackController : MonoBehaviour
{
    [Header("Laser Setup")]
    public BossLaser[] lasers = new BossLaser[4];
    public Transform[] firePoints = new Transform[4];

    [Header("Oscillation Settings")]
    public float rotationAmplitude = 30f; // Ángulo máximo en grados
    public float oscillationSpeed = 1f;   // Velocidad de oscilación

    private float time;

    void Start()
    {
        if (lasers.Length != 4 || firePoints.Length != 4)
        {
            Debug.LogError("Debes asignar 4 lasers y 4 firePoints.");
            enabled = false;
            return;
        }

        // Inicializa orientaciones base
        firePoints[0].localRotation = Quaternion.Euler(0, 0, 0);   // Frente
        firePoints[1].localRotation = Quaternion.Euler(0, 90, 0);   // Derecha
        firePoints[2].localRotation = Quaternion.Euler(0, 180, 0);   // Atrás
        firePoints[3].localRotation = Quaternion.Euler(0, -90, 0);   // Izquierda
    }

    void Update()
    {
        time += Time.deltaTime * oscillationSpeed;

        float angleA = Mathf.Sin(time) * rotationAmplitude;
        float angleB = Mathf.Sin(time + Mathf.PI) * rotationAmplitude;

        // Oscila sobre el eje Y (como aspas rotando)
        firePoints[0].localRotation = Quaternion.Euler(0, 0 + angleA, 0); // Frente
        firePoints[2].localRotation = Quaternion.Euler(0, 180 + angleA, 0); // Atrás
        firePoints[1].localRotation = Quaternion.Euler(0, 90 + angleB, 0); // Derecha
        firePoints[3].localRotation = Quaternion.Euler(0, -90 + angleB, 0); // Izquierda
    }
}
