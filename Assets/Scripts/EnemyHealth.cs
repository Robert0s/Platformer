using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int health = 1;
    [SerializeField] private float bounceForce = 12f;

    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int amount, Rigidbody2D playerRb)
    {
        health -= amount;

        if (health <= 0)
        {
            Die(playerRb);
        }
    }

    private void Die(Rigidbody2D playerRb)
    {
        // Bounce player in sus
        if (playerRb != null)
        {
            playerRb.linearVelocity = new Vector2(
                playerRb.linearVelocity.x,
                bounceForce
            );
        }

        // Pornim animatia de Hit
        anim.SetTrigger("Die");

        // Dezactivam colliderele ca inamicul sa nu mai faca damage
        // si sa nu mai fie impins in timp ce moare
        GetComponent<Collider2D>().enabled = false;
        GetComponent<EnemyPatrol>().enabled = false;

        // Distrugem dupa durata animatiei
        Destroy(gameObject, 0.30f);
        // 0.5f = durata animatiei de Hit in secunde, ajusteaza dupa animatie
    }
}