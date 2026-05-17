using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int inventorySize = 4;
    // Lista de sloturi din inventar
    private List<InventorySlot> slots = new List<InventorySlot>();

    // Slotul selectat curent (cu tastele 1,2,3,4)
    private int selectedSlot = 0;

    // Event apasat cand inventarul se schimba → InventoryUI se updateaza
    public event System.Action OnInventoryChanged;

    void Start()
    {
        // Initializam sloturile goale
        for (int i = 0; i < inventorySize; i++)
            slots.Add(new InventorySlot(null, 0));
    }

    void Update()
    {
        // Selectam slotul cu tastele 1-4
        for (int i = 0; i < inventorySize; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selectedSlot = i;
                OnInventoryChanged?.Invoke();
            }
        }
    }

    // Returnam itemul selectat curent
    public InventorySlot GetSelectedSlot() => slots[selectedSlot];
    public int GetSelectedIndex() => selectedSlot;
    public List<InventorySlot> GetSlots() => slots;

    // Adaugam un item in inventar
    public bool AddItem(Item item, int quantity = 1)
    {
        // Cautam un slot existent cu acelasi item
        foreach (var slot in slots)
        {
            if (slot.item == item && slot.quantity < item.maxStack)
            {
                slot.AddQuantity(quantity);
                OnInventoryChanged?.Invoke();
                return true;
            }
        }

        // Cautam un slot gol
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

        // Inventar plin
        Debug.Log("Inventar plin!");
        return false;
    }

    // Folosim itemul selectat (scadem cantitatea)
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