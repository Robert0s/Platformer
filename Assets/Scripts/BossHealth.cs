using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;
    private int currentHealth;

    [SerializeField] private Slider healthBar;
    [SerializeField] private float showRange = 15f;
    
    private Animator anim;
    private Transform player;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = maxHealth;
            healthBar.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if ( isDead || player == null || healthBar == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        healthBar.gameObject.SetActive(distance <= showRange);
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
        isDead = true;
        anim?.SetTrigger("Die");
        GetComponent<BossAI>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        // Ascundem health bar-ul cand moare boss-ul
        if (healthBar != null)
            healthBar.gameObject.SetActive(false);

        Destroy(gameObject, 1f);
    }
}