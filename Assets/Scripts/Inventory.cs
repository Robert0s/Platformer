using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int inventorySize = 4;
    
    private List<InventorySlot> slots = new List<InventorySlot>();

    
    private int selectedSlot = 0;

    
    public event System.Action OnInventoryChanged;

    void Start()
    {
        
        for (int i = 0; i < inventorySize; i++)
            slots.Add(new InventorySlot(null, 0));
    }

    void Update()
    {
        
        for (int i = 0; i < inventorySize; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selectedSlot = i;
                OnInventoryChanged?.Invoke();
            }
        }
    }

    
    public InventorySlot GetSelectedSlot() => slots[selectedSlot];
    public int GetSelectedIndex() => selectedSlot;
    public List<InventorySlot> GetSlots() => slots;

    
    public bool AddItem(Item item, int quantity = 1)
    {
        
        foreach (var slot in slots)
        {
            if (slot.item == item && slot.quantity < item.maxStack)
            {
                slot.AddQuantity(quantity);
                OnInventoryChanged?.Invoke();
                return true;
            }
        }

        
        foreach (var slot in slots)
        {
            if (slot.IsEmpty())
            {
                slot.item = item;
                slot.quantity = quantity;
                OnInventoryChanged?.Invoke();
                return true;
            }
        }

        
        Debug.Log("Inventar plin!");
        return false;
    }

    
    public bool UseSelectedItem()
    {
        var slot = slots[selectedSlot];
        if (slot.IsEmpty()) return false;

        

        slot.quantity--;
        if (slot.quantity <= 0)
        {
            slot.item = null;
            slot.quantity = 0;
        }

        OnInventoryChanged?.Invoke();
        return true;
    }
}