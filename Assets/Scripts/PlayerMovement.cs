using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;     // Tốc độ di chuyển
    public float jumpForce = 7f;     // Lực nhảy
    private Rigidbody2D rb;          // Rigidbody2D

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Lấy Rigidbody2D gắn trên object
    }

    void Update()
    {
        // Di chuyển ngang
        float horizontal = Input.GetAxis("Horizontal");
        Vector2 moveDirection = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);
        rb.linearVelocity = moveDirection;

        // Nhảy khi bấm W
        if (Input.GetKeyDown(KeyCode.W))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        // Lật hướng nhưng giữ nguyên scale gốc
        Vector3 currentScale = transform.localScale;
        if (horizontal < 0)
        {
            currentScale.x = -Mathf.Abs(currentScale.x);
            transform.localScale = currentScale;
        }
        else if (horizontal > 0)
        {
            currentScale.x = Mathf.Abs(currentScale.x);
            transform.localScale = currentScale;
        }

    }
}
