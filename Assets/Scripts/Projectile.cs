using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime = 3f;
    // Dupa cat timp se distruge daca nu loveste nimic

    [SerializeField] private float rotationSpeed = 720f;
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
        // Ignoram coliziunea cu playerul
        if (other.CompareTag("Player")) return;

        // Daca lovim un inamic, ii dam damage
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage, null);
            Destroy(gameObject);
            return;
        }

        // Daca lovim orice altceva (ex. ground), distrugem proiectilul
        if (!other.isTrigger)
            Destroy(gameObject);
    }
}