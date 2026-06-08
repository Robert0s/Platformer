using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject[] slots;
    

    [SerializeField] private Inventory inventory;
    
    
    [SerializeField] private Color selectedColor = Color.white;
    [SerializeField] private Color normalColor = new Color(1, 1, 1, 0.5f);

    void Start()
    {
        
        inventory.OnInventoryChanged += UpdateUI;
        UpdateUI();
    }

    void OnDestroy()
    {
        
        inventory.OnInventoryChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        var inventorySlots = inventory.GetSlots();
        int selectedIndex = inventory.GetSelectedIndex();

        for (int i = 0; i < slots.Length; i++)
        {
            
            Image slotBackground = slots[i].GetComponent<Image>();
            Image itemIcon = slots[i].transform.Find("Icon").GetComponent<Image>();
            TextMeshProUGUI quantityText = slots[i].transform.Find("Quantity").GetComponent<TextMeshProUGUI>();

            
            slotBackground.color = i == selectedIndex ? selectedColor : normalColor;

            
            if (i < inventorySlots.Count && !inventorySlots[i].IsEmpty())
            {
                itemIcon.sprite = inventorySlots[i].item.icon;
                itemIcon.enabled = true;
                quantityText.text = inventorySlots[i].quantity.ToString();
            }
            else
            {
                itemIcon.enabled = false;
                quantityText.text = "";
            }
        }
    }
}