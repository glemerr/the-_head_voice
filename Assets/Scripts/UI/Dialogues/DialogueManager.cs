using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI missionTitleText;
    public TextMeshProUGUI missionDescText;
    public Button nextButton;
    public AudioSource voiceSource;

    private DialogueNode[] nodes;
    private int currentIndex = 0;
    private bool isPaused = false;

    [Header("References to player")]
    private FirstPersonController playerController; // Reference to the player controller to pause movement
    private GunManager gunManager; // Reference to the gun manager, if needed
    public GameObject player; // Reference to the player GameObject, if needed
    private void Start()
    {
        playerController = player.GetComponent<FirstPersonController>();
        gunManager = player.GetComponentInChildren<GunManager>();

    }


    private void Awake()
    {
        dialoguePanel.SetActive(false);
        SetCursorState(isPaused);
        //nextButton.onClick.AddListener(DisplayNext);
    }

    /// <summary>
    /// Call this to start a sequence.
    /// </summary>
    public void StartDialogue(DialogueSequence seq)
    {
        nodes = seq.nodes;
        currentIndex = 0;
        dialoguePanel.SetActive(true);
        isPaused = true;
        SetCursorState(isPaused);
        DisplayNode(nodes[currentIndex]);
        Time.timeScale = 0f;
        gunManager.isPaused = isPaused; // Pause gun manager if needed
        playerController.cameraCanMove = !isPaused; // Disable player movement

    }

    private void DisplayNode(DialogueNode node)
    {
        speakerNameText.text = node.speakerName;
        dialogueText.text = node.dialogueText;
        missionTitleText.text = node.missionTitle;
        missionDescText.text = node.missionDescription;

        // play voice if present
        if (node.voiceClip != null)
        {
            voiceSource.clip = node.voiceClip;
            voiceSource.Play();
        }
    }

    public void DisplayNext()
    {
        currentIndex++;
        if (currentIndex < nodes.Length)
        {
            DisplayNode(nodes[currentIndex]);
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        isPaused = false;
        SetCursorState(isPaused);
        Time.timeScale = 1f;
        // TODO: trigger mission activation or next game event
        gunManager.isPaused = isPaused; // Pause gun manager if needed
        playerController.cameraCanMove = !isPaused;
    }
    
    void SetCursorState(bool showCursor)
    {
        Cursor.visible = showCursor;
        Cursor.lockState = showCursor ? CursorLockMode.None : CursorLockMode.Locked;
    }

}
