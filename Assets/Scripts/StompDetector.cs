using UnityEngine;

public class StompDetector : MonoBehaviour
{
    private EnemyHealth enemyHealth;

    void Awake()
    {
        
        enemyHealth = GetComponentInParent<EnemyHealth>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();

            if (playerRb != null && playerRb.linearVelocity.y < -0.1f)
            {
                
                enemyHealth.TakeDamage(1, playerRb);
            }
        }
    }
}