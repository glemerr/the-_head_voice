using UnityEngine;

public class WeakPoint : MonoBehaviour
{
    private LifeSystem lifeSystem;
    private BossController boss;

    public void Initialize(BossController bossController, float health)
    {
        boss = bossController;
        lifeSystem = GetComponent<LifeSystem>();

        if (lifeSystem == null)
        {
            Debug.LogError($"[WeakPoint] No se encontró LifeSystem en {gameObject.name}");
            return;
        }

        Debug.Log($"[WeakPoint] Inicializado con {health} de vida en {gameObject.name}");

        lifeSystem.Initialize(health, 0, health);
        lifeSystem.OnDeath.AddListener(NotifyBoss);

        // Verifica también que OnDeath se haya conectado
        Debug.Log("[WeakPoint] OnDeath listener conectado.");
    }

    private void NotifyBoss()
    {
        Debug.Log("[WeakPoint] WeakPoint destruido. Notificando al jefe...");
        
        if (boss != null)
        {
            boss.OnWeakPointDestroyed();
            Debug.Log("[WeakPoint] Boss notificado correctamente.");
        }
        else
        {
            Debug.LogWarning("[WeakPoint] No se asignó referencia al Boss.");
        }

        Destroy(gameObject);
        Debug.Log("[WeakPoint] GameObject destruido.");
    }

    // Extra opcional: método para probar manualmente daño
    private void OnMouseDown()
    {
        if (lifeSystem != null)
        {
            lifeSystem.TakeDamage(10);
            Debug.Log("[WeakPoint] Daño manual aplicado con click.");
        }
    }
}
