using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float airControlMultiplier = 0.8f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float lowJumpMultiplier = 2.5f;
    
    [SerializeField] private float fallMultiplier = 2f;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime = 0.15f;
    
    private float coyoteTimeCounter;

    [Header("Jump Buffer")]
    [SerializeField] private float jumpBufferTime = 0.1f;
    
    private float jumpBufferCounter;



    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckLeft;
    [SerializeField] private Transform groundCheckCenter;
    [SerializeField] private Transform groundCheckRight;
    [SerializeField] private LayerMask groundLayer;
    private const float groundCheckRadius = 0.1f;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;
    private float horizontalInput;
    private bool isFacingRight = true;

    private float baseMoveSpeed;
    public float BaseMoveSpeed => baseMoveSpeed;

    private bool wasGrounded;

    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        baseMoveSpeed = moveSpeed;
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        
        if (Input.GetButtonDown("Jump"))
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;

        
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpBufferCounter = 0f; 
            AudioManager.Instance?.PlaySFX(AudioManager.Instance.jumpSFX);
        }

        
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                rb.linearVelocity.y * 0.5f
            );
            coyoteTimeCounter = 0f;
        }

        
        if (horizontalInput > 0 && !isFacingRight) Flip();
        else if (horizontalInput < 0 && isFacingRight) Flip();

        
        anim.SetFloat("Speed", Mathf.Abs(horizontalInput));
        anim.SetBool("IsGrounded", isGrounded);
        anim.SetFloat("VerticalVelocity", rb.linearVelocity.y);
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckLeft.position, groundCheckRadius, groundLayer) || Physics2D.OverlapCircle(groundCheckCenter.position, groundCheckRadius, groundLayer) || Physics2D.OverlapCircle(groundCheckRight.position, groundCheckRadius, groundLayer);

        if (!wasGrounded && isGrounded)
            AudioManager.Instance?.PlaySFX(AudioManager.Instance.landSFX);
        wasGrounded = isGrounded;

        float currentSpeed = isGrounded ? moveSpeed : moveSpeed * airControlMultiplier;
        rb.linearVelocity = new Vector2(horizontalInput * currentSpeed, rb.linearVelocity.y);
        
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y
                * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y
                * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(
            -transform.localScale.x,
            transform.localScale.y,
            transform.localScale.z
        );
    }
}