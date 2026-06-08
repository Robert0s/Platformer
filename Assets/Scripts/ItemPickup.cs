using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private int quantity = 1;


    private bool collected = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;
        if (!other.CompareTag("Player")) return;

        Inventory inventory = other.GetComponent<Inventory>();
        if (inventory != null && inventory.AddItem(item, quantity))
        {
            
            collected = true;
            Destroy(gameObject);
            AudioManager.Instance?.PlaySFX(AudioManager.Instance.equipSFX);
        }
    }
}