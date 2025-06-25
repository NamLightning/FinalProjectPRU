using Unity.VisualScripting;
using UnityEngine;

public class PlayerColision : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerController playerController;
    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        playerController = GetComponent<PlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            gameManager.AddScore(1);
        }
        else if (collision.CompareTag("Trap"))
        {
            playerController.TakeHit(collision.transform.position);
        }

        else if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy == null || !enemy.isDead)
            {
                playerController.TakeHit(collision.transform.position);
            }
        }

    }
}
