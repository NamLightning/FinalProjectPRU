using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject hitbox;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null && !enemy.isDead)
            {
                enemy.isDead = true;
                Destroy(collision.gameObject);
                gameManager.AddScore(1);
            }
        }
    }
}
