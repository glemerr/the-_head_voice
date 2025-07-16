using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI quantityText;

    private InventorySlot currentSlot;

    public void Initialize(InventorySlot slot)
    {
        currentSlot = slot;
        iconImage.sprite = slot.collectable.icon;
        quantityText.text = slot.collectable.isStackable ? slot.quantity.ToString() : "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // Show collectable info
            InventoryUIManager.Instance.ShowCollectableInfo(currentSlot.collectable);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Use/drop collectable
            if (currentSlot.collectable.isUsable)
            {
                Debug.Log($"Using {currentSlot.collectable.displayName}");
                // Implement usage logic
            }
            else if (currentSlot.collectable.isDroppable)
            {
                InventoryManager.Instance.RemoveCollectable(currentSlot.collectable.collectableID);
                // Implement drop logic in the world
            }
        }
    }
}