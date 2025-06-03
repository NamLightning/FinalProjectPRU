using UnityEngine;

public class FrogEnemy : MonoBehaviour
{
    [SerializeField] private float jumpForceX = 5f;
    [SerializeField] private float jumpForceY = 7f;
    [SerializeField] private float distance = 5f;
    [SerializeField] private float jumpInterval = 2f;

    private Rigidbody2D rb;
    private Vector3 startPos;
    private bool movingRight = true;
    private float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        timer = jumpInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Jump();
            timer = jumpInterval;
        }

        float leftBound = startPos.x - distance;
        float rightBound = startPos.x + distance;

        if (transform.position.x >= rightBound && movingRight)
        {
            movingRight = false;
            Flip();
        }
        else if (transform.position.x <= leftBound && !movingRight)
        {
            movingRight = true;
            Flip();
        }
    }

    void Jump()
    {
        float direction = movingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(jumpForceX * direction, jumpForceY);
    }


    void Flip()
    {
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
