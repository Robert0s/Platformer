using UnityEngine;

[CreateAssetMenu(fileName = "NewPowerup", menuName = "Inventory/Powerup")]
public class PowerupItem : Item
{
    public enum PowerupType { SpeedBoost, Shield }
    
    public PowerupType powerupType;
    
    [SerializeField] public float duration = 5f;
    [SerializeField] public float speedMultiplier = 2f;
    // Folosit doar pentru SpeedBoost

    public void ApplyEffect(PowerupEffect effect)
    {
        switch (powerupType)
        {
            case PowerupType.SpeedBoost:
                effect.ActivateSpeedBoost(speedMultiplier, duration, icon);
                break;
            case PowerupType.Shield:
                effect.ActivateShield(duration, icon);
                break;
        }
    }
}