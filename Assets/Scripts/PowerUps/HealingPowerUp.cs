using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Healing")]
public class HealingPowerUp : PowerUp
{
    public float healAmount = 30f;

    public override void Activate(GameObject player)
    {
        LifeSystem lifeSystem = player.GetComponent<LifeSystem>();
        if (lifeSystem != null)
        {
            lifeSystem.Heal(healAmount);
        }
    }
}
