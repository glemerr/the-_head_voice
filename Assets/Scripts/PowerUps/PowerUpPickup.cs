using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PowerUpPickup : MonoBehaviour
{
    public PowerUp powerUp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (powerUp != null)
            {
                            NotificationManager.Instance.ShowItemNotification(
                powerUp.name, 
                powerUp.description, 
                powerUp.icon
            );
                powerUp.Activate(other.gameObject);
            }

            Destroy(gameObject); // elimina el objeto del mundo
        }
    }
}
