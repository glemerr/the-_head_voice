using UnityEngine;

[CreateAssetMenu(fileName = "New Collectable", menuName = "Inventory/CollectableObject")]
public class CollectableObject : ScriptableObject
{
    [Header("Identification")]
    public string collectableID;  // Unique ID (e.g., "house_key_01")
    public string displayName;

    [Header("Classification")]
    public CollectableType collectableType;

    [Header("Visuals")]
    public Sprite icon;
    [TextArea(3, 10)] public string description;
    
    [Header("Game Representation")]
    public GameObject worldPrefab;  // 3D model in the game world
    
    [Header("Stacking Settings")]
    public bool isStackable = false;
    public int maxStack = 1;  // Default to non-stackable
    
    [Header("Additional Properties")]
    public bool isUsable = false;
    public bool isDroppable = true;
    public float weight = 0.1f;
}