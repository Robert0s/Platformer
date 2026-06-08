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
        
        if (playerRb != null)
        {
            playerRb.linearVelocity = new Vector2(
                playerRb.linearVelocity.x,
                bounceForce
            );
        }

        
        anim.SetTrigger("Die");

        
        GetComponent<Collider2D>().enabled = false;
        GetComponent<EnemyPatrol>().enabled = false;

        
        Destroy(gameObject, 0.30f);
        
    }
}