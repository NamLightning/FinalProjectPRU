using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 30f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    private bool isGrounded;
    private Rigidbody2D rb;
    private GameManager gameManager;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindAnyObjectByType<GameManager>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (gameManager.IsGameOver()) return;
        HandleMovement();
        HandleJump();
    }
    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2 (moveInput* moveSpeed, rb.linearVelocity.y);
        if(moveInput >0) transform.localScale = new Vector3(1,1,1);
        if (moveInput < 0) transform.localScale = new Vector3(-1, 1, 1);
    }
    
    private void HandleJump()
    {
        if(Input.GetButtonDown("Jump")&&isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        isGrounded=Physics2D.OverlapCircle(groundCheck.position,0.2f, groundLayer);
    }
}
