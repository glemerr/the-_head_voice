using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 100f;
    public float attackPower = 10f;
    public float defense = 5f;

    [Header("Boss Settings")]
    public GameObject weakPointPrefab;
    public Transform weakPointParent;
    public float weakPointRadius = 1.5f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 30f;
    public float directionChangeIntervalMin = 2f;
    public float directionChangeIntervalMax = 4f;

    private LifeSystem lifeSystem;
    private int currentDirection = 1;
    private GameObject currentWeakPoint;
    private bool isDead = false;

    void Start()
    {
        lifeSystem = GetComponent<LifeSystem>();
        if (lifeSystem == null)
        {
            Debug.LogError("[BossController] No se encontró LifeSystem.");
            return;
        }

        lifeSystem.Initialize(maxHealth, 0, maxHealth);
        lifeSystem.OnDeath.AddListener(HandleBossDeath);

        Debug.Log("[BossController] Boss inicializado con " + maxHealth + " de vida.");

        SpawnWeakPoint();
        StartCoroutine(RotateDirectionRandomly());
    }

    void Update()
    {
        if (isDead) return;

        transform.Rotate(Vector3.up * rotationSpeed * currentDirection * Time.deltaTime);
    }

    private void SpawnWeakPoint()
    {
        if (weakPointPrefab == null || weakPointParent == null)
        {
            Debug.LogWarning("[BossController] Faltan referencias al prefab o al parent del WeakPoint.");
            return;
        }

        Vector3 localPos = Random.onUnitSphere * weakPointRadius;
        if (localPos.y < 0) localPos.y *= -1; // Solo mitad superior

        GameObject wp = Instantiate(weakPointPrefab, weakPointParent);
        wp.transform.localPosition = localPos;

        WeakPoint weakPoint = wp.GetComponent<WeakPoint>();
        if (weakPoint != null)
        {
            weakPoint.Initialize(this, maxHealth / 10f);
            Debug.Log("[BossController] WeakPoint generado con " + (maxHealth / 10f) + " de vida.");
        }
        else
        {
            Debug.LogWarning("[BossController] El prefab de WeakPoint no contiene el script WeakPoint.");
        }

        currentWeakPoint = wp;
    }

    public void OnWeakPointDestroyed()
    {
        if (isDead) return;

        float damage = maxHealth * 0.1f;
        Debug.Log("[BossController] WeakPoint destruido. Aplicando " + damage + " de daño al jefe.");
        lifeSystem.Subtract(damage);

        if (lifeSystem.Current > 0)
        {
            SpawnWeakPoint();
        }
        else
        {
            Debug.Log("[BossController] Vida del jefe llegó a cero tras destruir WeakPoint.");
            // En caso de que OnDeath no se haya invocado por algún error
            HandleBossDeath();
        }
    }

    private void HandleBossDeath()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("[BossController] El jefe ha sido derrotado. Se destruirá.");
        Destroy(gameObject);
    }

    private System.Collections.IEnumerator RotateDirectionRandomly()
    {
        while (!isDead)
        {
            float waitTime = Random.Range(directionChangeIntervalMin, directionChangeIntervalMax);
            yield return new WaitForSeconds(waitTime);
            currentDirection *= -1;
        }
    }
}
