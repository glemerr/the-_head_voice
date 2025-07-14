using UnityEngine;

[CreateAssetMenu(fileName = "DialogueNode", menuName = "History/DialogueNode")]
public class DialogueNode : ScriptableObject {
    public string speakerName;        // e.g. "La Abuela"
    [TextArea(3, 10)]
    public string dialogueText;       // the narration line
    public string missionTitle;       // optional
    [TextArea(1, 5)]
    public string missionDescription; // optional
    public AudioClip voiceClip;       // optional voice-over
}
