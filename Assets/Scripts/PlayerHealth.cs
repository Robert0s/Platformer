using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float invincibilityDuration = 1.5f;
    // Cat timp e invincibil playerul dupa ce ia damage
    [SerializeField] private float respawnDelay = 2f;

    private int currentHealth;
    private bool isInvincible = false;
    private bool isDead = false;
    private Animator anim;
    
    public int CurrentHealth => currentHealth;

    public int MaxHealth => maxHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        // Daca e invincibil, ignoram damage-ul
        if (isInvincible || isDead) return;

        currentHealth -= amount;
        Debug.Log("Health: " + currentHealth);

        anim.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Pornim I-frames
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    public void SetInvincible(bool value)
    {
        isInvincible = value;
        gameObject.layer = value ? LayerMask.NameToLayer("Invincible") : LayerMask.NameToLayer("Default");
    }

    private System.Collections.IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        // Asteptam durata I-frame-urilor
        gameObject.layer = LayerMask.NameToLayer("Invincible");
        yield return new WaitForSeconds(invincibilityDuration);
        gameObject.layer = LayerMask.NameToLayer("Default");
        isInvincible = false;
    }

    private void Die()
    {
        isDead = true;
        anim?.SetTrigger("Die");

        GetComponent<PlayerController>().enabled = false;
        GetComponent<PlayerAttack>().enabled = false;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);

        // Reincarcam scena curenta = respawn
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}