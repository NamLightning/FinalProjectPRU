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
            audioManager.PlaySFX(audioManager.playerHurt);
            playerController.TakeHit(collision.transform.position);
        }

        else if (collision.CompareTag("Enemy"))
        {
            audioManager.PlaySFX(audioManager.playerHurt);
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy == null || !enemy.isDead)
            {
                playerController.TakeHit(collision.transform.position);
            }
        }

      

    }
}
