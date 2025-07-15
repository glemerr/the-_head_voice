using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [SerializeField] private List<InventorySlot> slots = new List<InventorySlot>();
    [SerializeField] private int maxSlots = 24;

    public event Action OnInventoryUpdated;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool AddCollectable(CollectableObject collectable, int quantity = 1)
    {
        // Handle stackable collectables
        if (collectable.isStackable)
        {
            foreach (InventorySlot slot in slots)
            {
                if (slot.collectable.collectableID == collectable.collectableID)
                {
                    if (slot.quantity + quantity <= collectable.maxStack)
                    {
                        slot.AddToStack(quantity);
                        OnInventoryUpdated?.Invoke();
                        return true;
                    }
                    return false; // Stack full
                }
            }
        }

        // Add new collectable
        if (slots.Count >= maxSlots) return false; // Inventory full
        
        slots.Add(new InventorySlot(collectable, quantity));
        OnInventoryUpdated?.Invoke();
        return true;
    }

    public void RemoveCollectable(string collectableID, int quantity = 1)
    {
        InventorySlot slotToRemove = null;
        
        foreach (InventorySlot slot in slots)
        {
            if (slot.collectable.collectableID == collectableID)
            {
                slot.RemoveFromStack(quantity);
                
                if (slot.quantity <= 0)
                    slotToRemove = slot;
                
                break;
            }
        }

        if (slotToRemove != null) slots.Remove(slotToRemove);
        OnInventoryUpdated?.Invoke();
    }

    public List<InventorySlot> GetAllSlots() => new List<InventorySlot>(slots);
    public int GetEmptySlotCount() => maxSlots - slots.Count;
}