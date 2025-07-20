using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalSpawner : MonoBehaviour
{
    [Header("Next Level Settings")]
    [SerializeField] private string nextLevelName = "Loading";
    
    private float activationDelay;
    private GameObject portalEffect;
    private AudioClip portalSound;
    private bool isActive = false;

    public void Initialize(float delay, GameObject effect, AudioClip sound)
    {
        activationDelay = delay;
        portalEffect = effect;
        portalSound = sound;
        
        Invoke("ActivatePortal", activationDelay);
    }

    private void ActivatePortal()
    {
        isActive = true;
        
        // Efecto visual
        if (portalEffect != null)
        {
            Instantiate(portalEffect, transform.position, Quaternion.identity);
        }
        
        // Sonido
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && portalSound != null)
        {
            audioSource.PlayOneShot(portalSound);
        }
        
        // Animación (si hay animator)
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Activate");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isActive)
        {
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        Debug.Log("Loading next level: " + nextLevelName);
        // Implementar lógica de carga de nivel
        SceneManager.LoadScene(nextLevelName);
    }
}