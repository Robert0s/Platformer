using UnityEngine;
using System.Collections;

public class PowerupEffect : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerHealth playerHealth;
    private Coroutine speedBoostCoroutine;
    private Coroutine shieldCoroutine;
    private BuffUI buffUI;

    public bool IsSpeedBoostActive => speedBoostCoroutine != null;
    public bool IsShieldActive => shieldCoroutine != null;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerHealth = GetComponent<PlayerHealth>();
        buffUI = FindFirstObjectByType<BuffUI>();
    }

    // SPEED BOOST
    public void ActivateSpeedBoost(float multiplier, float duration, Sprite icon)
    {
        if (speedBoostCoroutine != null)
        {
            StopCoroutine(speedBoostCoroutine);
            playerController.MoveSpeed = playerController.BaseMoveSpeed;
        }
        speedBoostCoroutine = StartCoroutine(SpeedBoostCoroutine(multiplier, duration));
        buffUI?.ShowBuff("SpeedBoost", icon, duration);
    }

    private IEnumerator SpeedBoostCoroutine(float multiplier, float duration)
    {
        playerController.MoveSpeed = playerController.BaseMoveSpeed * multiplier;
        yield return new WaitForSeconds(duration);
        playerController.MoveSpeed = playerController.BaseMoveSpeed;
        speedBoostCoroutine = null;
    }

    // SHIELD
    public void ActivateShield(float duration, Sprite icon)
    {
        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
            playerHealth.SetInvincible(false);
        }
        shieldCoroutine = StartCoroutine(ShieldCoroutine(duration));
        buffUI?.ShowBuff("Shield", icon, duration);
    }

    private IEnumerator ShieldCoroutine(float duration)
    {
        playerHealth.SetInvincible(true);
        yield return new WaitForSeconds(duration);
        playerHealth.SetInvincible(false);
        shieldCoroutine = null;
    }
}