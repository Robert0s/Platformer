using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime = 3f;
    

    [SerializeField] private float rotationSpeed = 720f;

    [SerializeField] private bool isBossProjectile = false;

    

    [Header("Homing")]
    [Tooltip("Homing incet spre player +- momentan sare in sus.")]
    [SerializeField] private bool homing = false;
    [Tooltip("cat de repede merge spre player in grade pe secunda.")]
    [SerializeField] private float homingTurnSpeed = 120f;
    [Tooltip("cat timp corecteaza proiectilul.")]
    [SerializeField] private float homingDuration = 0.6f;
    [Tooltip("am incercat ceva tine minte sa actualizezi functia.")]
    [SerializeField] private float maxHomingAngle = 45f;

    private Rigidbody2D rb;
    private Transform homingTarget;
    private float homingTimer;
    private Vector2 initialDirection;

    void Start()
    {
        
        Destroy(gameObject, lifetime);

        if (homing)
        {
            rb = GetComponent<Rigidbody2D>();
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                homingTarget = playerObj.transform;

            homingTimer = homingDuration;
            initialDirection = rb != null ? rb.linearVelocity.normalized : (Vector2)transform.right;
        }
    }

    void Update()
    {
        
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        if (homing && rb != null && homingTarget != null && homingTimer > 0f)
        {
            homingTimer -= Time.deltaTime;

            Vector2 currentVelocity = rb.linearVelocity;
            Vector2 desiredDirection = (homingTarget.position - transform.position).normalized;

            //trebuie sa lucrez la ecuatia asta
            desiredDirection = Vector3.RotateTowards(initialDirection, desiredDirection,
                maxHomingAngle * Mathf.Deg2Rad, 0f);

            Vector2 newDirection = Vector3.RotateTowards(currentVelocity.normalized, desiredDirection,
                homingTurnSpeed * Mathf.Deg2Rad * Time.deltaTime, 0f);

            rb.linearVelocity = newDirection * currentVelocity.magnitude;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isBossProjectile)
        {
            
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerHealth>()?.TakeDamage(damage);
                Destroy(gameObject);
            }
            else if (!other.isTrigger)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            
            if (other.CompareTag("Player")) return;

            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            BossHealth bossHealth = other.GetComponent<BossHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage, null);
                Destroy(gameObject);
                return;
            }

            if (bossHealth != null)
            {
                bossHealth.TakeDamage(damage);
                Destroy(gameObject);
                return;
            }

            if (!other.isTrigger)
                Destroy(gameObject);
        }
    }
}