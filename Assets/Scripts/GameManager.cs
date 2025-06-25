using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    private int score = 0;
    [SerializeField] private TextMeshProUGUI  scoreText;
    [SerializeField] private GameObject gameOverUI;
    private bool isGameOver = false;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

   /* [SerializeField]
    private GameObject[] heartIcons;*/

    private PlayerController playerController;

    [SerializeField]
    private Image[] heartIcons;

    [SerializeField]
    private Sprite fullHeart;
    [SerializeField]
    private Sprite emptyHeart;

    void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        Time.timeScale = 1f;
        UpdateScore();
        gameOverUI.SetActive(false);
        currentHealth = maxHealth;
        UpdateHearts();
    }

    void Update()
    {
        
    }

    public void AddScore(int points)
    {
        if (!isGameOver)
        {
            score += points;
            UpdateScore();
        }
    }

    private void UpdateScore()
    {
        scoreText.text = score.ToString();
    }
    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        score = 0;

        playerController.PlayDeathAnimation();

        StartCoroutine(ShowGameOverAfterDelay(1.5f));
    }

    private System.Collections.IEnumerator ShowGameOverAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        Time.timeScale = 0;
        gameOverUI.SetActive(true);
    }

    public void TakeDamage(int damage)
    {
        if (isGameOver) return;

        currentHealth -= damage;
        UpdateHearts();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            
            GameOver();
        }
    }
    private void UpdateHearts()
    {
        for (int i = 0; i < heartIcons.Length; i++)
        {
            if (i < currentHealth)
            {
                heartIcons[i].sprite = fullHeart;
            }
            else
            {
                heartIcons[i].sprite = emptyHeart;
            }
        }
    }


    public void RestartGame()
    {
        isGameOver=false;
        score = 0;
        UpdateScore() ;
        Time.timeScale = 1;
        SceneManager.LoadScene("Nam");
        mainMenu.SetActive(false);
        StartGame();
    }



    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void StartGame()
    {
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        mainMenu.SetActive(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 0f;
    }
    public void pauseGameMenu()
    {   
        mainMenu.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale= 0f ;
    }

    public void ResumeGame()
    {
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Nam");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
