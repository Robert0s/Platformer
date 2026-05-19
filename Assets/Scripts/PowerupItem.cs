using UnityEngine;

[CreateAssetMenu(fileName = "NewPowerup", menuName = "Inventory/Powerup")]
public class PowerupItem : Item
{
    public enum PowerupType { SpeedBoost, Shield }
    
    public PowerupType powerupType;
    
    [SerializeField] private float duration = 5f;
    [SerializeField] private float speedMultiplier = 2f;
    // Folosit doar pentru SpeedBoost

    public void ApplyEffect(PowerupEffect effect)
    {
        switch (powerupType)
        {
            case PowerupType.SpeedBoost:
                effect.ActivateSpeedBoost(speedMultiplier, duration);
                break;
            case PowerupType.Shield:
                effect.ActivateShield(duration);
                break;
        }
    }
}