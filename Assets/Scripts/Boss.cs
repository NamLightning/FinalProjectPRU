using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Boss Stats")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Movement Parameters")]
    public float patrolSpeed = 1f;        // Giảm tốc độ patrol
    public float chaseSpeed = 1.5f;       // Tốc độ chase chậm hơn
    public float boundaryDistance = 5f;   // Khoảng cách ranh giới từ startPos
    private Vector3 startPos;
    private bool movingRight = false;

    [Header("Attack Parameters")]
    [SerializeField] private float meleeAttackCooldown = 2f;
    [SerializeField] private float meleeRange = 2f;
    [SerializeField] private int meleeDamage = 25;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance = 1f;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;

    [Header("References")]
    public UnityEngine.Transform player;
    public Animator animator;
    public Rigidbody2D rb;

    // State machine
    private enum BossState
    {
        Patrolling,
        Chasing,
        MeleeAttack,
        TakeHit,
        Dead
    }

    private BossState currentState;
    private float meleeAttackTimer = Mathf.Infinity;
    private bool isDead = false;
    private bool isAttacking = false;
    private bool deathTriggered = false; // Thêm flag để đảm bảo trigger die chỉ gọi 1 lần
    private float distanceToPlayer;
    private Vector2 directionToPlayer;

    void Start()
    {
        currentHealth = maxHealth;
        currentState = BossState.Patrolling;
        startPos = transform.position;

        // Tự động tìm player nếu chưa được gán
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        // Cố định Rigidbody2D settings
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.linearDamping = 8f;
            rb.angularDamping = 10f;
            rb.freezeRotation = true;
        }
    }

    void Update()
    {
        if (isDead) return;

        // Cập nhật cooldown timers
        meleeAttackTimer += Time.deltaTime;

        if (player != null)
        {
            CalculatePlayerDistance();
        }

        UpdateStateMachine();
        UpdateAnimations();
    }

    void CalculatePlayerDistance()
    {
        directionToPlayer = (player.position - transform.position).normalized;
        distanceToPlayer = Vector2.Distance(transform.position, player.position);
    }

    void UpdateStateMachine()
    {
        switch (currentState)
        {
            case BossState.Patrolling:
                HandlePatrollingState();
                break;
            case BossState.Chasing:
                HandleChasingState();
                break;
            case BossState.MeleeAttack:
                HandleMeleeAttackState();
                break;
            case BossState.TakeHit:
                HandleTakeHitState();
                break;
        }
    }

    void HandlePatrollingState()
    {
        // Kiểm tra nếu player trong phạm vi boundary
        if (player != null && IsPlayerInBoundary())
        {
            currentState = BossState.Chasing;
            return;
        }

        // Patrol logic trong phạm vi boundary
        float leftBound = startPos.x - boundaryDistance;
        float rightBound = startPos.x + boundaryDistance;

        if (movingRight)
        {
            transform.Translate(Vector2.right * patrolSpeed * Time.deltaTime);
            if (transform.position.x >= rightBound)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            transform.Translate(Vector2.left * patrolSpeed * Time.deltaTime);
            if (transform.position.x <= leftBound)
            {
                movingRight = true;
                Flip();
            }
        }
    }

    void HandleChasingState()
    {
        // Kiểm tra nếu player ra khỏi boundary
        if (player == null || !IsPlayerInBoundary())
        {
            currentState = BossState.Patrolling;
            return;
        }

        // Kiểm tra tấn công tầm gần
        if (PlayerInMeleeRange() && meleeAttackTimer >= meleeAttackCooldown)
        {
            currentState = BossState.MeleeAttack;
            return;
        }

        // Nếu player trong tầm tấn công nhưng chưa đến cooldown, dừng lại chờ
        if (PlayerInMeleeRange())
        {
            // Boss dừng lại và chỉ flip hướng nhìn về player
            FlipTowardsPlayer();
            return;
        }

        // Chase player nhưng chỉ trong boundary
        ChasePlayer();
    }

    void HandleMeleeAttackState()
    {
        if (!isAttacking)
        {
            StartCoroutine(PerformMeleeAttack());
        }
    }

    void HandleTakeHitState()
    {
        // Trạng thái này sẽ được xử lý bởi animation event
    }

    bool IsPlayerInBoundary()
    {
        if (player == null) return false;

        float leftBound = startPos.x - boundaryDistance;
        float rightBound = startPos.x + boundaryDistance;

        return player.position.x >= leftBound && player.position.x <= rightBound;
    }

    void ChasePlayer()
    {
        // Flip sprite để hướng về player
        FlipTowardsPlayer();

        // Di chuyển về phía player nhưng kiểm tra boundary
        if (distanceToPlayer > meleeRange)
        {
            // CHỈ lấy hướng X, bỏ qua Y để không bị lấn xuống
            float moveDirectionX = directionToPlayer.x > 0 ? 1f : -1f;
            Vector2 moveDirection = new Vector2(moveDirectionX * chaseSpeed, 0f);

            Vector3 newPosition = transform.position + (Vector3)moveDirection * Time.deltaTime;

            // Kiểm tra boundary trước khi di chuyển
            float leftBound = startPos.x - boundaryDistance;
            float rightBound = startPos.x + boundaryDistance;

            // Giới hạn di chuyển trong boundary - CHỈ DI CHUYỂN TRỤC X
            if (newPosition.x >= leftBound && newPosition.x <= rightBound)
            {
                rb.linearVelocity = new Vector2(moveDirectionX * chaseSpeed, 0f); // Luôn Y = 0
            }
            else
            {
                rb.linearVelocity = new Vector2(0f, 0f); // Dừng lại nếu ra ngoài boundary
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(0f, 0f); // Dừng lại khi đủ gần
        }
    }

    void FlipTowardsPlayer()
    {
        if (directionToPlayer.x > 0.1f && !movingRight)
        {
            movingRight = true;
            Flip();
        }
        else if (directionToPlayer.x < -0.1f && movingRight)
        {
            movingRight = false;
            Flip();
        }
    }

    void Flip()
    {
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    IEnumerator PerformMeleeAttack()
    {
        isAttacking = true;

        // Trigger animation
        animator.SetTrigger("attack");

        // Đợi một chút trước khi gây damage (để sync với animation)
        yield return new WaitForSeconds(0.5f);

        // Kiểm tra xem player vẫn còn trong tầm tấn công không
        if (PlayerInMeleeRange())
        {
            DealMeleeDamageToPlayer(meleeDamage);
        }

        meleeAttackTimer = 0;

        // Đợi animation kết thúc
        yield return new WaitForSeconds(0.5f);

        isAttacking = false;
        currentState = BossState.Chasing;
    }

    void DealMeleeDamageToPlayer(int damage)
    {
        if (player != null)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeHit(transform.position);
            }
        }
    }

    private bool PlayerInMeleeRange()
    {
        if (boxCollider == null || player == null) return false;

        // Tính toán vị trí attack box
        float direction = movingRight ? 1f : -1f;
        Vector2 attackBoxCenter = (Vector2)transform.position + new Vector2(meleeRange * direction, 0);
        Vector2 attackBoxSize = new Vector2(meleeRange * 2f, boxCollider.bounds.size.y);

        Collider2D hit = Physics2D.OverlapBox(attackBoxCenter, attackBoxSize, 0f, playerLayer);
        return hit != null;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Chỉ trigger take hit nếu không đang trong trạng thái TakeHit
            if (currentState != BossState.TakeHit)
            {
                animator.SetTrigger("hurt");
                currentState = BossState.TakeHit;

                // Tự động chuyển về trạng thái phù hợp sau khi take hit
                StartCoroutine(RecoverFromHit());
            }
        }
    }

    IEnumerator RecoverFromHit()
    {
        yield return new WaitForSeconds(0.5f);
        if (!isDead && currentState == BossState.TakeHit) // Kiểm tra thêm currentState
        {
            // Nếu player còn trong boundary thì chase, không thì patrol
            if (player != null && IsPlayerInBoundary())
            {
                currentState = BossState.Chasing;
            }
            else
            {
                currentState = BossState.Patrolling;
            }
        }
    }

    void Die()
    {
        if (isDead) return; // Ngăn gọi Die() nhiều lần

        isDead = true;
        currentState = BossState.Dead;

        // Chỉ trigger die một lần với flag bổ sung
        if (!deathTriggered)
        {
            deathTriggered = true;
            animator.SetTrigger("die");
            Debug.Log("Boss death triggered"); // Debug để kiểm tra
        }

        // Disable collider để không nhận damage nữa
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // Destroy boss sau một khoảng thời gian
        Destroy(gameObject, 3f);
    }

    void UpdateAnimations()
    {
        // Cập nhật animation parameters
        bool isMoving = (currentState == BossState.Patrolling) ||
                       (currentState == BossState.Chasing && !PlayerInMeleeRange());
        animator.SetBool("moving", isMoving && !isAttacking);
    }

    // Animation Events (gọi từ animation clips)
    public void OnAttackComplete()
    {
        isAttacking = false;
        if (currentState == BossState.MeleeAttack)
        {
            currentState = BossState.Chasing;
        }
    }

    public void OnTakeHitComplete()
    {
        if (!isDead && currentState == BossState.TakeHit) // Kiểm tra thêm currentState
        {
            if (player != null && IsPlayerInBoundary())
            {
                currentState = BossState.Chasing;
            }
            else
            {
                currentState = BossState.Patrolling;
            }
        }
    }

    public void OnDeathComplete()
    {
        // Logic khi boss chết hoàn toàn
    }

    // Gizmos để debug
    void OnDrawGizmosSelected()
    {
        // Vẽ boundary area
        Gizmos.color = Color.green;
        Vector3 pos = Application.isPlaying ? startPos : transform.position;
        Vector3 leftBound = new Vector3(pos.x - boundaryDistance, pos.y - 2f, pos.z);
        Vector3 rightBound = new Vector3(pos.x + boundaryDistance, pos.y - 2f, pos.z);
        Vector3 leftTop = new Vector3(pos.x - boundaryDistance, pos.y + 2f, pos.z);
        Vector3 rightTop = new Vector3(pos.x + boundaryDistance, pos.y + 2f, pos.z);

        // Vẽ hình chữ nhật boundary
        Gizmos.DrawLine(leftBound, rightBound);
        Gizmos.DrawLine(leftBound, leftTop);
        Gizmos.DrawLine(rightBound, rightTop);
        Gizmos.DrawLine(leftTop, rightTop);

        // Vẽ tầm tấn công
        if (boxCollider != null)
        {
            Gizmos.color = Color.red;
            float direction = movingRight ? 1f : -1f;
            Vector2 attackBoxCenter = (Vector2)transform.position + new Vector2(meleeRange * direction, 0);
            Vector2 attackBoxSize = new Vector2(meleeRange * 2f, boxCollider.bounds.size.y);
            Gizmos.DrawWireCube(attackBoxCenter, attackBoxSize);
        }
    }
}