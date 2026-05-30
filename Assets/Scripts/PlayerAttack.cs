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

        if (selectedSlot.IsEmpty()) return;

        PowerupItem powerup = selectedSlot.item as PowerupItem;
        if (powerup != null)
        {
            PowerupEffect effect = GetComponent<PowerupEffect>();
            if (effect != null)
            {
                if (powerup.powerupType == PowerupItem.PowerupType.SpeedBoost && effect.IsSpeedBoostActive)
                {
                    Debug.Log("Speed boost deja activ!");
                    return;
                }
                if (powerup.powerupType == PowerupItem.PowerupType.Shield && effect.IsShieldActive)
                {
                    Debug.Log("Shield deja activ!");
                    return;
                }

                // Activam efectul cu icon pentru BuffUI
                if (powerup.powerupType == PowerupItem.PowerupType.SpeedBoost)
                    effect.ActivateSpeedBoost(powerup.speedMultiplier, powerup.duration, powerup.icon);
                else if (powerup.powerupType == PowerupItem.PowerupType.Shield)
                    effect.ActivateShield(powerup.duration, powerup.icon);
            }
            inventory.UseSelectedItem();
            return;
        }

        if (selectedSlot.item.projectilePrefab == null) return;

        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;

        GameObject projectile = Instantiate(
            selectedSlot.item.projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = direction * projectileSpeed;

        inventory.UseSelectedItem();
    }
}