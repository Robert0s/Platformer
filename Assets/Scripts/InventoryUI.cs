using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject[] slots;
    // Array cu GameObject-urile sloturilor din UI

    [SerializeField] private Inventory inventory;

    // Culoarea slotului selectat si a celui normal
    [SerializeField] private Color selectedColor = Color.white;
    [SerializeField] private Color normalColor = new Color(1, 1, 1, 0.5f);

    void Start()
    {
        // Ne abonam la eventul de schimbare al inventarului
        inventory.OnInventoryChanged += UpdateUI;
        UpdateUI();
    }

    void OnDestroy()
    {
        // Ne dezabonam cand obiectul e distrus, buna practica
        inventory.OnInventoryChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        var inventorySlots = inventory.GetSlots();
        int selectedIndex = inventory.GetSelectedIndex();

        for (int i = 0; i < slots.Length; i++)
        {
            // Gasim componentele din slot
            Image slotBackground = slots[i].GetComponent<Image>();
            Image itemIcon = slots[i].transform.Find("Icon").GetComponent<Image>();
            TextMeshProUGUI quantityText = slots[i].transform.Find("Quantity").GetComponent<TextMeshProUGUI>();

            // Highlight slot selectat
            slotBackground.color = i == selectedIndex ? selectedColor : normalColor;

            // Afisam itemul daca slotul nu e gol
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