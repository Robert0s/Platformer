using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificam daca e playerul
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Capcanele dau damage instant, fara I-frames speciale
                // I-frames din PlayerHealth se aplica normal
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}