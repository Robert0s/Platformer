using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 10f;
    

    [SerializeField] private Transform firePoint;
    

    private Inventory inventory;
    private bool isFacingRight = true;

    void Awake()
    {
        inventory = GetComponent<Inventory>();
    }

    void Update()
    {
        
        isFacingRight = transform.localScale.x > 0;

        
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

        AudioManager.Instance?.PlaySFX(AudioManager.Instance.attackSFX);
        inventory.UseSelectedItem();
    }
}