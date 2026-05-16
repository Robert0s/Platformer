using UnityEngine;

public class WhompTrap : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Calculam directia din care a venit playerul
        // Diferenta dintre pozitia playerului si pozitia whompului
        Vector2 direction = other.transform.position - transform.position;

        // Determinam din ce directie a venit playerul
        // Comparam componentele x si y ca sa stim daca e lovit
        // mai mult pe orizontala sau verticala
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Lovitura orizontala
            if (direction.x > 0)
            {
                anim.SetTrigger("RightHit");
                // Playerul e la dreapta whompului → lovitura din dreapta
            }
            else
            {
                anim.SetTrigger("LeftHit");
                // Playerul e la stanga whompului → lovitura din stanga
            }
        }
        else
        {
            // Lovitura verticala
            if (direction.y > 0)
            {
                anim.SetTrigger("TopHit");
                // Playerul e deasupra whompului → lovitura de sus
            }
            else
            {
                anim.SetTrigger("BottomHit");
                // Playerul e dedesubt → lovitura de jos
            }
        }

        // Dam damage playerului
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
            playerHealth.TakeDamage(damageAmount);
    }
}