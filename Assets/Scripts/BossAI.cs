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
    [Tooltip("Timpul (secunde) dintre pornirea animatiei de shoot si momentul in care mana e intinsa spre FirePoint. Regleaza in Play Mode pana cand proiectilul porneste exact din mana bossului.")]
    [SerializeField] private float shootDelay = 0.4f;

    [Header("Search")]
    [SerializeField] private float searchDuration = 3f;

    [Header("Flight Collision")]
    [Tooltip("Layer-ul folosit de tilemap-urile/obstacolele cu coliziune (implicit 'Ground').")]
    [SerializeField] private LayerMask groundLayer = 1 << 6;
    [SerializeField] private float obstacleCheckDistance = 1.2f;

    private BossState currentState = BossState.Patrol;
    private Transform player;
    private Vector3 startPosition;
    private Vector3 lastKnownPlayerPosition;
    private float attackTimer;
    private bool isAttacking = false;
    private float searchTimer;
    private float patrolDirection = 1f;
    private SpriteRenderer sr;
    private Animator anim;
    private Collider2D col;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
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

    // Cast-eaza chiar forma corpului bossului (nu doar o raza subtire din centru), ca sa
    // detectam obstacolul inainte sa intre corpul lui in el, indiferent de directie.
    private bool IsBlocked(Vector2 direction, out RaycastHit2D hit)
    {
        if (col != null)
        {
            hit = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, direction, obstacleCheckDistance, groundLayer);
        }
        else
        {
            hit = Physics2D.Raycast(transform.position, direction, obstacleCheckDistance, groundLayer);
        }

        return hit.collider != null;
    }

    // Daca directia dorita e blocata de un obstacol (tilemap), incearca sa ocoleasca pe sus/jos.
    // Bossul zboara, deci se poate ridica sau cobori ca sa treaca de un perete/coloana.
    private Vector2 AvoidObstacles(Vector2 desiredDirection)
    {
        if (desiredDirection == Vector2.zero) return desiredDirection;

        if (!IsBlocked(desiredDirection, out _))
            return desiredDirection;

        if (!IsBlocked(Vector2.up, out _))
            return Vector2.up;

        if (!IsBlocked(Vector2.down, out _))
            return Vector2.down;

        // Inconjurat din toate partile - mai bine stam pe loc decat sa intram in perete
        return Vector2.zero;
    }

    private void Patrol()
    {
        Vector2 moveDirection = Vector2.right * patrolDirection;

        if (IsBlocked(moveDirection, out _))
        {
            patrolDirection *= -1f;
            FlipBoss(patrolDirection < 0f);
        }
        else
        {
            transform.Translate(moveDirection * patrolSpeed * Time.deltaTime);
        }

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
        transform.Translate(AvoidObstacles(direction) * chaseSpeed * Time.deltaTime);

        FlipBoss(player.position.x < transform.position.x);

        anim?.SetBool("IsAttacking", false);
    }

    private void AttackPlayer()
    {
        if (player == null) return;

        anim?.SetBool("IsAttacking", true);
        FlipBoss(player.position.x < transform.position.x);

        // Atacul curent (animatie + tragere) e deja in desfasurare - il lasam sa se termine
        // complet, indiferent daca playerul iese din raza intre timp. Nu re-evaluam starea
        // pana nu se termina ciclul (asta elimina si dodge-ul de ultim moment si animation lock-ul).
        if (isAttacking) return;

        if (attackTimer <= 0f)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Abia acum, intre doua atacuri, decidem daca mai pornim unul nou sau trecem la Chase
            if (distanceToPlayer > attackRange)
            {
                currentState = BossState.Chase;
                anim?.SetBool("IsAttacking", false);
                return;
            }

            isAttacking = true;
            StartCoroutine(ShootAfterDelay(shootDelay));
            attackTimer = attackCooldown;
        }
    }

    private System.Collections.IEnumerator ShootAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Shoot();

        // Atacul s-a terminat - abia acum putem trece la Chase/Search/Patrol daca e cazul
        isAttacking = false;
    }

    private void Shoot()
    {
        if (projectilePrefab == null || firePoint == null) return;

        bool facingRight = transform.localScale.x > 0;
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Oglindim local scale (nu doar SpriteRenderer.flipX), ca sa se intoarca odata cu spriteul
        // si colliderul (care are un offset mare) - altfel hitboxul ramane "in urma" cand
        // proiectilul e tras spre dreapta si nu mai loveste playerul.
        Vector3 projectileScale = projectile.transform.localScale;
        projectileScale.x = facingRight ? -Mathf.Abs(projectileScale.x) : Mathf.Abs(projectileScale.x);
        projectile.transform.localScale = projectileScale;

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
            transform.Translate(AvoidObstacles(direction) * patrolSpeed * Time.deltaTime);
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