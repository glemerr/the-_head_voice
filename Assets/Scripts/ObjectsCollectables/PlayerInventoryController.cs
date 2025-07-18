using UnityEngine;

public class PlayerInventoryController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InventoryUIManager.Instance.ToggleInventory();
        }
    }
}