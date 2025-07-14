using UnityEngine;
using System.Collections.Generic;

public class PowerUpsManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Power-Up Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float spawnProbability = 0.2f; // Probability of
    public List<PowerUpPickup> powerUpPickups = new List<PowerUpPickup>();

    [Header("Spawn Settings")]
    public Transform defaultSpawnPoint;

    void Start()
    {
        if (defaultSpawnPoint == null)
        {
            defaultSpawnPoint = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnPowerUp(PowerUpPickup powerUp, Transform spawnPoint = null)
    {
        if (powerUp == null)
        {
            Debug.LogWarning("PowerUp is null.");
            return;
        }

        if (spawnPoint == null)
            spawnPoint = defaultSpawnPoint;

        PowerUpPickup powerUpInstance = Instantiate(powerUp, spawnPoint.position, spawnPoint.rotation);
        powerUpInstance.transform.SetParent(spawnPoint);

        Debug.Log($"Power-Up spawned: {powerUp.name}");
    }

    public void TrySpawnRandomPowerUp(Vector3 position)
    {
        if (powerUpPickups == null || powerUpPickups.Count == 0) return;
        defaultSpawnPoint.position = position;
        float roll = Random.value;
        if (roll > spawnProbability)
        {
            //Debug.Log("No power-up spawned (roll failed).");
            return;
        }

        int randomIndex = Random.Range(0, powerUpPickups.Count);
        PowerUpPickup selectedPowerUpPickup = powerUpPickups[randomIndex];
        Debug.Log($"Selected Power-Up: {selectedPowerUpPickup.name}");
        SpawnPowerUp(selectedPowerUpPickup, defaultSpawnPoint);
    }
    
    public void SetProbability(float newProb)
    {
        spawnProbability = Mathf.Clamp01(newProb);
    }
}
