using Unity.VisualScripting;
using UnityEngine;

public class PlayerColision : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerController playerController;
    AudioManager audioManager;
    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        playerController = GetComponent<PlayerController>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Coin"))
        {
            audioManager.PlaySFX(audioManager.EarnCoin);
            Destroy(collision.gameObject);
            gameManager.AddScore(1);
        }
        else if (collision.CompareTag("Trap"))
        {
            playerController.PlayDeathAnimation();
            gameManager.GameOver();
        }
        else if (collision.CompareTag("Enemy")) 
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy == null || !enemy.isDead)
            {
                playerController.PlayDeathAnimation();
                gameManager.GameOver();
            }
            
        }
    }
}
