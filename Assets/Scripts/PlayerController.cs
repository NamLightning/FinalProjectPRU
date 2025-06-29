using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 30f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private GameObject swordHitbox;

    [SerializeField] private float attackDuration = 0.2f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    private bool isInvincible = false;
    [SerializeField] private float invincibilityDuration = 1f;
    [SerializeField] private float knockbackForce = 10f;


    //Audio 
    AudioManager audioManager;


    private bool isTouchingWall;



    private Animator anim;
    private bool isAttacking = false;
    private bool isGrounded;
    private Rigidbody2D rb;
    [SerializeField]private GameManager gameManager;
    private GameObject currentTeleporter;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindAnyObjectByType<GameManager>();
        anim = GetComponent<Animator>();
        swordHitbox.SetActive(false);
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (gameManager.IsGameOver())
        {
            Debug.Log("Game is over, input blocked");
            return;
        }
        Debug.Log("Update running, Time.timeScale: " + Time.timeScale);
        HandleMovement();
        HandleJump();
        HandleAttack();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.pauseGameMenu();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentTeleporter != null)
            {
                transform.position = currentTeleporter.GetComponent<Teleporter>().GetDestination().position;
            }
        }
    }
    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Debug.Log("Move Input: " + moveInput);
        rb.linearVelocity = new Vector2 (moveInput* moveSpeed, rb.linearVelocity.y);
        if(moveInput >0) transform.localScale = new Vector3(1,1,1);
        if (moveInput < 0) transform.localScale = new Vector3(-1, 1, 1);

        anim.SetFloat("Speed", Mathf.Abs(moveInput));
    }
    
    private void HandleJump()
    {
        if(Input.GetButtonDown("Jump")&&isGrounded)
        {

            audioManager.PlaySFX(audioManager.jumpSound);
            if (isGrounded)
            {
               
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            else if (!isGrounded && isTouchingWall)
            {
                
                float direction = transform.localScale.x > 0 ? -1 : 1;
                rb.linearVelocity = new Vector2(direction * moveSpeed, jumpForce);
            }

        }
        isGrounded=Physics2D.OverlapCircle(groundCheck.position,0.2f, groundLayer);
        anim.SetBool("IsJumping", !isGrounded);
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            
            StartCoroutine(PerformAttack());
            audioManager.PlaySFX(audioManager.attackSound);
        }
    }

    private System.Collections.IEnumerator PerformAttack()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");

        swordHitbox.SetActive(true);

        yield return new WaitForSeconds(attackDuration);

        swordHitbox.SetActive(false); 
        isAttacking = false;
    }

    public void PlayDeathAnimation()
    {
        anim.SetTrigger("Death");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        {
            currentTeleporter = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        {
            if (collision.gameObject == currentTeleporter)
            {
                currentTeleporter = null;
            }
        }
    }
}
