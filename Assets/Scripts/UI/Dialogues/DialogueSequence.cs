using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSequence", menuName = "History/DialogueSequence")]
public class DialogueSequence : ScriptableObject {
    public DialogueNode[] nodes;
}