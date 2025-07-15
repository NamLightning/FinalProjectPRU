using Unity.VisualScripting;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    [SerializeField] private int bossDamage = 1; // Damage cho boss
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

        // ✅ THÊM: Xử lý Boss
        else if (collision.CompareTag("Boss"))
        {
            Boss boss = collision.GetComponent<Boss>();
            if (boss != null)
            {
                boss.TakeDamage(bossDamage);
                Debug.Log($"Player deals {bossDamage} damage to boss! Boss health: {boss.currentHealth}");
            }
        }
    }
}

