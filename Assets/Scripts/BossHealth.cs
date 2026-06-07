using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;
    private int currentHealth;

    [SerializeField] private Slider healthBar;
    // Slider UI pentru boss health bar
    private Animator anim;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = maxHealth;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (healthBar != null)
            healthBar.value = currentHealth;

        anim?.SetTrigger("Hit");

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        anim?.SetTrigger("Die");
        GetComponent<BossAI>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 1f);
    }
}