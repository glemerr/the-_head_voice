using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance;

    [Header("UI References")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform slotsContainer;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Text itemNameText;
    [SerializeField] private Text itemDescriptionText;
    [SerializeField] private Image itemIcon;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        InventoryManager.Instance.OnInventoryUpdated += UpdateUI;
        inventoryPanel.SetActive(false);
    }

    public void ToggleInventory()
    {
        bool isOpening = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(isOpening);
        
        if (isOpening) UpdateUI();
    }

    void UpdateUI()
    {
        // Clear existing slots
        foreach (Transform child in slotsContainer) Destroy(child.gameObject);

        // Create new slots
        foreach (InventorySlot slot in InventoryManager.Instance.GetAllSlots())
        {
            GameObject slotObj = Instantiate(slotPrefab, slotsContainer);
            slotObj.GetComponent<InventorySlotUI>().Initialize(slot);
        }
    }

    public void ShowCollectableInfo(CollectableObject collectable)
    {
        itemNameText.text = collectable.displayName;
        itemDescriptionText.text = collectable.description;
        itemIcon.sprite = collectable.icon;
    }
}