using UnityEngine;

public class Carrier : MonoBehaviour
{


    [Header("Health System")]
    [SerializeField] protected LifeSystem lifeSystem;
    [SerializeField] private GameOverManager gameOverUI;

    void Start()
{
    lifeSystem.OnDeath.AddListener(gameOverUI.ShowGameOverScreen);
}
    public virtual void TakeDamage(float damage)
    {
        lifeSystem.TakeDamage(damage);
    }

    public virtual void Heal(float amount)
    {
        lifeSystem.Heal(amount);
    }

    public bool IsAlive => lifeSystem.Current > lifeSystem.Min;
}