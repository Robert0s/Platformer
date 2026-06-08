using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private float moveSpeed = 3f;
    
    [SerializeField] private float patrolDistance = 4f;

    [Header("Combat")]
    [SerializeField] private int damageAmount = 1;

    private Vector3 startPosition;
    
    private float direction = 1f;
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        
        startPosition = transform.position;
    }

    void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        
        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

        
        float distanceFromStart = transform.position.x - startPosition.x;

        
        if (distanceFromStart > patrolDistance)
        {
            direction = -1f;
            sr.flipX = true; 
        }
        else if (distanceFromStart < -patrolDistance)
        {
            direction = 1f;
            sr.flipX = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
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