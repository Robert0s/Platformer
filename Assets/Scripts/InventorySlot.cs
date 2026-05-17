using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int quantity;

    public InventorySlot(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

    // Adaugam o bucata in slot
    public void AddQuantity(int amount)
    {
        quantity += amount;
        if (quantity > item.maxStack)
            quantity = item.maxStack;
    }

    public bool IsEmpty() => item == null;
}