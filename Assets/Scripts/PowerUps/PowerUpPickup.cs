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
                powerUp.Activate(other.gameObject);
            }

            Destroy(gameObject); // elimina el objeto del mundo
        }
    }
}
