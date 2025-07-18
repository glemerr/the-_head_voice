using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance { get; private set; }

    [Header("Core References")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform slotsContainer;
    [SerializeField] private GameObject slotPrefab;

    [Header("Details Panel")]
    [SerializeField] private Image detailIcon;
    [SerializeField] private TextMeshProUGUI detailName;
    [SerializeField] private TextMeshProUGUI detailType;
    [SerializeField] private TextMeshProUGUI detailDescription;

    private CollectableObject currentSelected;
    private InventorySlotUI lastHighlightedSlot;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryUpdated += UpdateUI;
        }
        else
        {
            Debug.LogError("InventoryManager instance not found!");
        }

        inventoryPanel.SetActive(false);
    }

    void UpdateUI()
    {
        ClearSlots();
        PopulateSlots();
        ClearSelection();
    }

    void ClearSlots()
    {
        foreach (Transform child in slotsContainer)
        {
            Destroy(child.gameObject);
        }
    }

    void PopulateSlots()
    {
        if (InventoryManager.Instance != null)
        {
            foreach (InventorySlot slot in InventoryManager.Instance.GetAllSlots())
            {
                // Skip null slots
                if (slot == null || slot.collectable == null) continue;
                
                GameObject slotObj = Instantiate(slotPrefab, slotsContainer);
                InventorySlotUI slotUI = slotObj.GetComponent<InventorySlotUI>();
                
                if (slotUI != null)
                {
                    slotUI.Initialize(slot, this);
                }
            }
        }
    }

    public void SelectCollectable(CollectableObject collectable, InventorySlotUI slotUI)
    {
        // Validate before selection
        if (collectable == null || slotUI == null) return;
        
        // Clear previous highlight
        if (lastHighlightedSlot != null && lastHighlightedSlot != slotUI)
        {
            lastHighlightedSlot.SetHighlight(false);
        }

        currentSelected = collectable;
        lastHighlightedSlot = slotUI;
        
        // Update details panel
        if (detailIcon != null) detailIcon.sprite = collectable.icon;
        if (detailName != null) detailName.text = collectable.displayName;
        if (detailType != null) detailType.text = collectable.collectableType.ToString();
        if (detailDescription != null) detailDescription.text = collectable.description;
    }

    void ClearSelection()
    {
        currentSelected = null;
        
        // Reset details panel
        if (detailIcon != null) detailIcon.sprite = null;
        if (detailName != null) detailName.text = "";
        if (detailType != null) detailType.text = "";
        if (detailDescription != null) detailDescription.text = "";
        
        // Clear highlight
        if (lastHighlightedSlot != null)
        {
            lastHighlightedSlot.SetHighlight(false);
            lastHighlightedSlot = null;
        }
    }

    public void ToggleInventory()
    {
        bool isOpening = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(isOpening);
        
        if (isOpening) 
        {
            UpdateUI();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            ClearSelection();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}