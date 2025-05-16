// Guardar como Assets/Scripts/Game/PlayerController.cs
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

    [Header("Scoring")]
    private float maxHeightReached = 0f;
    public float GetMaxHeightReached() => maxHeightReached;

    [Header("Death Handling")]

    public float fallDeathYPosition = -5f;

    public delegate void PlayerDiedAction(float finalScore);
    public static event PlayerDiedAction OnPlayerDied;

    private Camera mainCamera;

  

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }
    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;

        HandleInput();
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
    public void InitializePositionAndHeight(Vector3 startPosition)
    {
        transform.position = startPosition;
        maxHeightReached = startPosition.y; 
        isGrounded = true;
        if (rb == null) rb = GetComponent<Rigidbody2D>(); 
        rb.linearVelocity = Vector2.zero; 
    }
    void HandleInput()
    {

        horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && isGrounded)
            {
                Jump();
            }


            if (touch.position.x < Screen.width / 2)
            {
                horizontalInput = -1f;
            }
            else if (touch.position.x > Screen.width / 2)
            {
                horizontalInput = 1f;
            }
        }
        else if (Input.GetAxis("Horizontal") == 0 && Input.touchCount == 0)
        {
            horizontalInput = 0f; 
        }
    }

    void HandleMovement()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); 
        rb.AddForce(new Vector2(0f, jumpForce), (ForceMode2D)ForceMode.Impulse);
        isGrounded = false;
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
        OnPlayerDied?.Invoke(maxHeightReached);
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
            // Verifica si el contacto es desde arriba
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.7f)
                {
                    isGrounded = true;
                    break;
                }
            }
        }
    }
}