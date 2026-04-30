using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private float moveSpeed = 3f;
    // Distanta maxima pe care o parcurge inamicul in fiecare directie
    [SerializeField] private float patrolDistance = 4f;

    [Header("Combat")]
    [SerializeField] private int damageAmount = 1;

    private Vector3 startPosition;
    // Directia curenta: 1 = dreapta, -1 = stanga
    private float direction = 1f;
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        // Salvam pozitia initiala ca punct de referinta pentru patrulare
        startPosition = transform.position;
    }

    void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        // Miscam inamicul in directia curenta
        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

        // Calculam cat de departe suntem fata de pozitia initiala
        float distanceFromStart = transform.position.x - startPosition.x;

        // Daca am depasit distanta maxima, intoarcem directia
        if (distanceFromStart > patrolDistance)
        {
            direction = -1f;
            sr.flipX = true; // Intoarcem sprite-ul
        }
        else if (distanceFromStart < -patrolDistance)
        {
            direction = 1f;
            sr.flipX = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Daca inamicul atinge playerul, ii dam damage
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}