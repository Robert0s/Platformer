using UnityEngine;
using System.Collections;

public class PowerupEffect : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerHealth playerHealth;
    private Coroutine speedBoostCoroutine;
    private Coroutine shieldCoroutine;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    // SPEED BOOST
    public void ActivateSpeedBoost(float multiplier, float duration)
    {
        if (speedBoostCoroutine != null)
        {
            StopCoroutine(speedBoostCoroutine);
            playerController.MoveSpeed = playerController.BaseMoveSpeed;
        }
        speedBoostCoroutine = StartCoroutine(SpeedBoostCoroutine(multiplier, duration));
    }

    private IEnumerator SpeedBoostCoroutine(float multiplier, float duration)
    {
        playerController.MoveSpeed = playerController.BaseMoveSpeed * multiplier;
        yield return new WaitForSeconds(duration);
        playerController.MoveSpeed = playerController.BaseMoveSpeed;
        speedBoostCoroutine = null;
    }

    // SHIELD
    public void ActivateShield(float duration)
    {
        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
            playerHealth.SetInvincible(false);
        }
        shieldCoroutine = StartCoroutine(ShieldCoroutine(duration));
    }

    private IEnumerator ShieldCoroutine(float duration)
    {
        playerHealth.SetInvincible(true);
        yield return new WaitForSeconds(duration);
        playerHealth.SetInvincible(false);
        shieldCoroutine = null;
    }
}