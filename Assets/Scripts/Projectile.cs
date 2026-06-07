using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime = 3f;
    // Dupa cat timp se distruge daca nu loveste nimic

    [SerializeField] private float rotationSpeed = 720f;

    [SerializeField] private bool isBossProjectile = false;

    // Cat de repede se roteste shuriken-ul in zbor

    [Header("Homing")]
    [Tooltip("Daca e activat, proiectilul isi corecteaza usor directia spre player pentru un timp limitat dupa lansare.")]
    [SerializeField] private bool homing = false;
    [Tooltip("Cat de repede se roteste directia spre player (grade pe secunda). Valoare mica = corectie usoara, nu un missile perfect.")]
    [SerializeField] private float homingTurnSpeed = 120f;
    [Tooltip("Cat timp dupa lansare mai corecteaza proiectilul directia inainte sa zboare drept.")]
    [SerializeField] private float homingDuration = 0.6f;
    [Tooltip("Cat de mult se poate abate tinta de homing fata de directia initiala de lansare (grade). Limiteaza abaterea ca proiectilul sa nu 'ocoleasca' tinta cand aceasta se misca brusc (ex. player sare exact cand e lansat).")]
    [SerializeField] private float maxHomingAngle = 45f;

    private Rigidbody2D rb;
    private Transform homingTarget;
    private float homingTimer;
    private Vector2 initialDirection;

    void Start()
    {
        // Distrugem proiectilul dupa lifetime secunde
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
        // Rotim shuriken-ul pentru efect vizual
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        if (homing && rb != null && homingTarget != null && homingTimer > 0f)
        {
            homingTimer -= Time.deltaTime;

            Vector2 currentVelocity = rb.linearVelocity;
            Vector2 desiredDirection = (homingTarget.position - transform.position).normalized;

            // Limitam cat de mult se poate abate tinta fata de directia initiala, ca proiectilul
            // sa nu se "intoarca" si sa ocoleasca playerul cand acesta sare brusc chiar la spawn
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
            // Proiectilul boss-ului loveste playerul
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
            // Proiectilul playerului loveste inamicii
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