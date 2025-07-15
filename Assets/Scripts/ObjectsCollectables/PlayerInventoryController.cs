using UnityEngine;

public class PlayerInventoryController : MonoBehaviour
{
    [Header("Hotkeys")]
    [SerializeField] private KeyCode inventoryKey = KeyCode.Tab;
    [SerializeField] private KeyCode useKey = KeyCode.E;
    [SerializeField] private KeyCode dropKey = KeyCode.Q;

    void Update()
    {
        HandleInventoryInput();
    }

    void HandleInventoryInput()
    {
        if (Input.GetKeyDown(inventoryKey))
        {
            // Toggle inventory UI
            InventoryUIManager.Instance.ToggleInventory();
        }
    }
}