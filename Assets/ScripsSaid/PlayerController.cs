
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 7f;
    public float jumpForce = 15f;
    private Rigidbody2D rb;
    private float horizontalInput;

    [Header("Ground Check")]
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.25f;
    public LayerMask platformLayer;
    private bool isGrounded;

    [Header("Touch Settings")]
    [Range(0.1f, 0.4f)]
    public float leftRegion = 0.3f;
    [Range(0.6f, 0.9f)]
    public float rightRegion = 0.7f;

    [Header("Scoring")]
    private float maxHeightReached = 0f;
    public float GetMaxHeightReached() => maxHeightReached;

    [Header("Death Handling")]
    public float fallDeathYPosition = -5f;
    [SerializeField] private GameEventFloat onPlayerDiedEvent;

    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;

        HandleTouchInput();
        CheckIfGrounded();
        UpdateHeight();
        CheckForFall();
    }

    void FixedUpdate()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        HandleMovement();
    }

    void HandleTouchInput()
    {
        horizontalInput = 0f;
        bool leftTouched = false;
        bool rightTouched = false;

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                float touchX = touch.position.x;
                float screenWidth = Screen.width;

                if (touchX < screenWidth * leftRegion)
                {
                    leftTouched = true;
                }
                else if (touchX > screenWidth * rightRegion)
                {
                    rightTouched = true;
                }
                else 
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        Jump();
                    }
                }
            }
        }

        if (leftTouched) horizontalInput = -1f;
        else if (rightTouched) horizontalInput = 1f;
    }

    void HandleMovement()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(new Vector2(0f, jumpForce), (ForceMode2D)ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void CheckIfGrounded()
    {
        if (rb.linearVelocity.y <= 0.1f)
        {
            Collider2D hitCollider = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, platformLayer);
            isGrounded = hitCollider != null;
        }
        else
        {
            isGrounded = false;
        }
    }

    void UpdateHeight()
    {
        if (transform.position.y > maxHeightReached)
        {
            maxHeightReached = transform.position.y;
        }
    }

    void CheckForFall()
    {
        if (mainCamera != null)
        {
            fallDeathYPosition = mainCamera.transform.position.y - mainCamera.orthographicSize - 2f;
        }

        if (transform.position.y < fallDeathYPosition)
        {
            Die();
        }
    }

    void Die()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;

        Debug.Log("Player Died. Max Height: " + maxHeightReached);
        gameObject.SetActive(false);

        onPlayerDiedEvent.Raise(ScoreManager.Instance.CurrentScore);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & platformLayer) != 0)
        {
            bool newPlatformTouched = true;

            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.7f)
                {
                    isGrounded = true;

                    if (collision.gameObject.TryGetComponent<Platform>(out var platform))
                    {
                        if (!platform.HasBeenTouched)
                        {
                            platform.MarkAsTouched();
                            ScoreManager.Instance.AddScore(1);
                        }
                    }
                    break;
                }
            }
        }
    }

    public void InitializePositionAndHeight(Vector3 startPosition)
    {
        transform.position = startPosition;
        maxHeightReached = startPosition.y;
        isGrounded = true;
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
    }
}