using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;
    [HideInInspector] public float platformVelocityX = 0f;

    [Header("Jump")]
    [SerializeField] float jumpForce = 7f;
    [SerializeField] float coyoteTime = 0.1f;
    [SerializeField] float jumpBufferTime = 0.1f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float checkRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;

    [Header("Game Over")]
    public float fallDeathY = -10f;
    private GameOverManager gameOverManager;

    // [DITAMBAHKAN AUDIO]
    [Header("Audio")]
    public AudioClip jumpSound;
    private AudioSource audioSource;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;

    float move;
    bool isGrounded;
    float coyoteTimer;
    float jumpBufferTimer;

    private bool isKnockedBack = false;
    private float knockbackTimer = 0f;
    public float knockbackDuration = 0.3f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        gameOverManager = FindObjectOfType<GameOverManager>();

        // [DITAMBAHKAN AUDIO]
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        move = Input.GetAxisRaw("Horizontal");

        if (move != 0)
            sr.flipX = move < 0;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        if (isGrounded)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
            jumpBufferTimer = jumpBufferTime;
        else
            jumpBufferTimer -= Time.deltaTime;

        if (jumpBufferTimer > 0 && coyoteTimer > 0)
        {
            Jump();
            jumpBufferTimer = 0;
        }

        if (Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        if (transform.position.y < fallDeathY)
        {
            gameOverManager?.TriggerGameOver();
        }

        anim.SetFloat("Speed", Mathf.Abs(move));
        anim.SetBool("IsGrounded", isGrounded);
    }

    void FixedUpdate()
    {
        if (isKnockedBack)
        {
            knockbackTimer -= Time.fixedDeltaTime;
            if (knockbackTimer <= 0)
                isKnockedBack = false;
            return;
        }

        rb.linearVelocity = new Vector2(move * moveSpeed + platformVelocityX, rb.linearVelocity.y);
    }

    public void ApplyKnockback(Vector2 force)
    {
        isKnockedBack = true;
        knockbackTimer = knockbackDuration;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        // [DITAMBAHKAN AUDIO] Mainkan suara lompat
        if (jumpSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }
}