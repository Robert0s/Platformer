using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime = 3f;
    // Dupa cat timp se distruge daca nu loveste nimic

    [SerializeField] private float rotationSpeed = 720f;

    [SerializeField] private bool isBossProjectile = false;

    // Cat de repede se roteste shuriken-ul in zbor

    void Start()
    {
        // Distrugem proiectilul dupa lifetime secunde
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Rotim shuriken-ul pentru efect vizual
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
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