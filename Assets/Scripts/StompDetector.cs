using UnityEngine;

public class StompDetector : MonoBehaviour
{
    private EnemyHealth enemyHealth;

    void Awake()
    {
        // Luam EnemyHealth de pe parintele inamicului
        enemyHealth = GetComponentInParent<EnemyHealth>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Verificam ca playerul cade de sus (viteza Y negativa)
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();

            if (playerRb != null && playerRb.linearVelocity.y < -0.1f)
            {
                // Playerul cade pe inamic → damage
                enemyHealth.TakeDamage(1, playerRb);
            }
        }
    }
}