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

        
        Vector2 direction = other.transform.position - transform.position;

        
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            
            if (direction.x > 0)
            {
                anim.SetTrigger("RightHit");
                
            }
            else
            {
                anim.SetTrigger("LeftHit");
                
            }
        }
        else
        {
            
            if (direction.y > 0)
            {
                anim.SetTrigger("TopHit");
                
            }
            else
            {
                anim.SetTrigger("BottomHit");
                
            }
        }

        
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
            playerHealth.TakeDamage(damageAmount);
    }
}