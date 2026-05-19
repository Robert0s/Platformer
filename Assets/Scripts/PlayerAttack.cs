using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 10f;
    // Viteza cu care zboara proiectilul

    [SerializeField] private Transform firePoint;
    // Punctul din care e aruncat proiectilul (child gol pe player)

    private Inventory inventory;
    private bool isFacingRight = true;

    void Awake()
    {
        inventory = GetComponent<Inventory>();
    }

    void Update()
    {
        // Sincronizam directia cu PlayerController
        // Verificam scala pe X ca sa stim spre ce parte priveste
        isFacingRight = transform.localScale.x > 0;

        // Atacam cu click stanga sau tasta E
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private void Attack()
    {
        var selectedSlot = inventory.GetSelectedSlot();

        // Verificam ca slotul selectat nu e gol si are prefab de proiectil
        if (selectedSlot.IsEmpty()) return;
        PowerupItem powerup = selectedSlot.item as PowerupItem;
        if (powerup != null)
        {
            // Activam efectul powerup-ului
            PowerupEffect effect = GetComponent<PowerupEffect>();
            if (effect != null)
                powerup.ApplyEffect(effect);

            inventory.UseSelectedItem();
            return;
        }
        if (selectedSlot.item.projectilePrefab == null) return;

        // Calculam directia in care priveste playerul
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;

        // Instantiem proiectilul la pozitia firePoint
        GameObject projectile = Instantiate(
            selectedSlot.item.projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        // Setam viteza proiectilului
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = direction * projectileSpeed;

        // Consumam un item din inventar
        inventory.UseSelectedItem();
    }
}