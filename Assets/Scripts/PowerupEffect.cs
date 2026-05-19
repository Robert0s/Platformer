using UnityEngine;
using System.Collections;

public class PowerupEffect : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerHealth playerHealth;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    // SPEED BOOST
    public void ActivateSpeedBoost(float multiplier, float duration)
    {
        StartCoroutine(SpeedBoostCoroutine(multiplier, duration));
    }

    private IEnumerator SpeedBoostCoroutine(float multiplier, float duration)
    {
        // Salvam viteza originala si o inmultim
        float originalSpeed = playerController.MoveSpeed;
        playerController.MoveSpeed *= multiplier;

        yield return new WaitForSeconds(duration);

        // Revenim la viteza originala
        playerController.MoveSpeed = originalSpeed;
    }

    // SHIELD
    public void ActivateShield(float duration)
    {
        StartCoroutine(ShieldCoroutine(duration));
    }

    private IEnumerator ShieldCoroutine(float duration)
    {
        // Activam invincibilitatea
        playerHealth.SetInvincible(true);

        yield return new WaitForSeconds(duration);

        playerHealth.SetInvincible(false);
    }
}