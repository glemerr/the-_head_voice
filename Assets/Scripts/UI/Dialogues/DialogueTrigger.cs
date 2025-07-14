using UnityEngine;

public class DialogueTrigger : MonoBehaviour


{
    [Header("Secuencia de Diálogo")]
    public DialogueSequence introSecuencia;        // Arrastra aquí “IntroSecuencia”

    [Header("Referencias")]
    [SerializeField] private DialogueManager dialogueManager;

    private void Awake()
    {
        //dialogueManager = FindFirstObjectByType<DialogueManager>();
        if (dialogueManager == null)
            Debug.LogError("No se encontró DialogueManager en la escena.");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Asume que el jugador tiene tag "Player"
        if (other.CompareTag("Player"))
        {
            dialogueManager.StartDialogue(introSecuencia);
            // Desactivar este trigger para que no vuelva a dispararse
            enabled = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // Aquí podrías manejar la lógica al salir del trigger, si es necesario
        // Por ejemplo, podrías reactivar el trigger si quieres que se pueda volver a activar
        // enabled = true;
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject); // Destruye el objeto del trigger al salir
        }
    }
}
