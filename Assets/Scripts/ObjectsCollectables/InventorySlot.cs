[System.Serializable]
public class InventorySlot
{
    public CollectableObject collectable;
    public int quantity;

    public InventorySlot(CollectableObject collectable, int quantity)
    {
        this.collectable = collectable;
        this.quantity = quantity;
    }

    public void AddToStack(int amount) => quantity += amount;
    public void RemoveFromStack(int amount) => quantity -= amount;
}