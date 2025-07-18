using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI References")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI quantity;
    [SerializeField] private GameObject highlight;

    private InventorySlot slot;
    private InventoryUIManager uiManager;

    public void Initialize(InventorySlot slot, InventoryUIManager manager)
    {
        // Null check all critical components
        if (slot == null || slot.collectable == null || manager == null)
        {
            Debug.LogError("InventorySlotUI initialization failed: Missing required references");
            return;
        }

        this.slot = slot;
        this.uiManager = manager;
        
        // Set UI elements with null checks
        if (icon != null) icon.sprite = slot.collectable.icon;
        if (quantity != null) quantity.text = slot.collectable.isStackable ? slot.quantity.ToString() : "";
        if (highlight != null) highlight.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (slot == null || slot.collectable == null || uiManager == null) return;
        uiManager.SelectCollectable(slot.collectable, this);
        SetHighlight(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetHighlight(false);
    }

    public void SetHighlight(bool state)
    {
        if (highlight != null) highlight.SetActive(state);
    }
}