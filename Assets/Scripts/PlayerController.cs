using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 30f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private GameObject attackHitbox;
    [SerializeField] private float attackDuration = 0.2f;

    private Animator anim;
    private bool isGrounded;
    private Rigidbody2D rb;
<<<<<<< Updated upstream
    private GameManager gameManager;
=======
    private bool isAttacking = false;
    [SerializeField]private GameManager gameManager;
>>>>>>> Stashed changes
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gameManager = FindAnyObjectByType<GameManager>();
        attackHitbox.SetActive(false);
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (gameManager.IsGameOver()) return;
        HandleMovement();
        HandleJump();
<<<<<<< Updated upstream
=======
        HandleAttack();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.pauseGameMenu();
        }

    
>>>>>>> Stashed changes
    }
    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2 (moveInput* moveSpeed, rb.linearVelocity.y);
        if(moveInput >0) transform.localScale = new Vector3(1,1,1);
        if (moveInput < 0) transform.localScale = new Vector3(-1, 1, 1);

        anim.SetFloat("Speed", Mathf.Abs(moveInput));
    }
    
    private void HandleJump()
    {
        if(Input.GetButtonDown("Jump")&&isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        isGrounded=Physics2D.OverlapCircle(groundCheck.position,0.2f, groundLayer);

        anim.SetBool("IsJumping", !isGrounded);
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(PerformAttack());
        }
    }

    private System.Collections.IEnumerator PerformAttack()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");

        attackHitbox.SetActive(true);

        yield return new WaitForSeconds(attackDuration);

        attackHitbox.SetActive(false);
        isAttacking = false;
    }
}
