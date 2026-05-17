using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private int quantity = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Inventory inventory = other.GetComponent<Inventory>();
        if (inventory != null && inventory.AddItem(item, quantity))
        {
            // Itemul a fost adaugat cu succes → distrugem pickup-ul
            Destroy(gameObject);
        }
    }
}