using UnityEngine;

public class BossAI : MonoBehaviour
{
    public enum BossState { Patrol, Chase, Attack, Search }

    [Header("Patrol")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float patrolDistance = 5f;

    [Header("Chase")]
    [SerializeField] private float chaseSpeed = 4f;
    [SerializeField] private float detectionRange = 8f;

    [Header("Attack")]
    [SerializeField] private float attackRange = 6f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Search")]
    [SerializeField] private float searchDuration = 3f;

    private BossState currentState = BossState.Patrol;
    private Transform player;
    private Vector3 startPosition;
    private Vector3 lastKnownPlayerPosition;
    private float attackTimer;
    private float searchTimer;
    private float patrolDirection = 1f;
    private SpriteRenderer sr;
    private Animator anim;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        startPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        attackTimer -= Time.deltaTime;

        switch (currentState)
        {
            case BossState.Patrol:
                Patrol();
                CheckForPlayer();
                break;
            case BossState.Chase:
                Chase();
                break;
            case BossState.Attack:
                AttackPlayer();
                break;
            case BossState.Search:
                Search();
                break;
        }
    }

    private void FlipBoss(bool facingLeft)
    {
        Vector3 scale = transform.localScale;
        scale.x = facingLeft ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    private void Patrol()
    {
        transform.Translate(Vector2.right * patrolDirection * patrolSpeed * Time.deltaTime);

        float distanceFromStart = transform.position.x - startPosition.x;
        if (distanceFromStart > patrolDistance)
        {
            patrolDirection = -1f;
            FlipBoss(true);
        }
        else if (distanceFromStart < -patrolDistance)
        {
            patrolDirection = 1f;
            FlipBoss(false);
        }

        anim?.SetBool("IsAttacking", false);
    }

    private void CheckForPlayer()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            lastKnownPlayerPosition = player.position;
            currentState = BossState.Chase;
        }
    }

    private void Chase()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > detectionRange)
        {
            currentState = BossState.Search;
            searchTimer = searchDuration;
            anim?.SetBool("IsAttacking", false);
            return;
        }

        if (distanceToPlayer <= attackRange)
        {
            currentState = BossState.Attack;
            return;
        }

        lastKnownPlayerPosition = player.position;
        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * chaseSpeed * Time.deltaTime);

        FlipBoss(player.position.x < transform.position.x);

        anim?.SetBool("IsAttacking", false);
    }

    private void AttackPlayer()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > attackRange)
        {
            currentState = BossState.Chase;
            anim?.SetBool("IsAttacking", false);
            return;
        }

        anim?.SetBool("IsAttacking", true);
        FlipBoss(player.position.x < transform.position.x);

        if (attackTimer <= 0f)
        {
            Shoot();
            attackTimer = attackCooldown;
        }
    }

    private void Shoot()
    {
        Debug.Log("Shoot called! projectilePrefab: " + projectilePrefab + " firePoint: " + firePoint);

        if (projectilePrefab == null || firePoint == null) return;

        Vector2 direction = (player.position - firePoint.position).normalized;
        Debug.Log("Direction: " + direction);

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = direction * 8f;
    }

    private void Search()
    {
        float distanceToLastPos = Vector2.Distance(transform.position, lastKnownPlayerPosition);

        if (distanceToLastPos > 0.5f)
        {
            Vector2 direction = (lastKnownPlayerPosition - transform.position).normalized;
            transform.Translate(direction * patrolSpeed * Time.deltaTime);
            FlipBoss(lastKnownPlayerPosition.x < transform.position.x);
            anim?.SetBool("IsAttacking", false);
        }
        else
        {
            anim?.SetBool("IsAttacking", false);
            searchTimer -= Time.deltaTime;

            if (searchTimer <= 0f)
                currentState = BossState.Patrol;
        }

        CheckForPlayer();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}