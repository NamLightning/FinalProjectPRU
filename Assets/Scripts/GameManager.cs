using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Sprites;
public class GameManager : MonoBehaviour
{
    private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOverUI;
    private bool isGameOver = false;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private int maxHealth = 3;
    private int currentHealth;
    public int CurrentScore => score;

    private PlayerController playerController;

    [SerializeField]
    private Image[] heartIcons;

    [SerializeField]
    private Sprite fullHeart;
    [SerializeField]
    private Sprite emptyHeart;
    [SerializeField] private GameObject inGameUI;

    //Audio 
    AudioManager audioManager;

    private void Awake()
    {
       
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

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

    public void UpdateScore()
    {
        scoreText.text = score.ToString();
    }
    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        score = 0;

        playerController.PlayDeathAnimation();
        inGameUI.SetActive(false);
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
    public void UpdateHearts()
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



    //Update
    public bool SpendScore(int amount)
    {
        if (isGameOver) return false;

        if (score >= amount)
        {
            score -= amount;
            UpdateScore();
            return true;
        }
        else
        {
            Debug.Log("Not enough score!");
            return false;
        }
    }

    public void Heal(int amount)
    {
        if (isGameOver) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHearts();
    }


    public void RestartGame()
    {
        isGameOver = false;
        score = 0;
        currentHealth = maxHealth;
        UpdateScore();
        UpdateHearts();
        Time.timeScale = 1;
        SceneManager.LoadScene("Hoang");
        mainMenu.SetActive(false);
        inGameUI.SetActive(true);
        StartGame();
    }



    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void StartGame()
    {
        if (audioManager != null)
        {
            audioManager.StopMusic(); 
        }

        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        inGameUI.SetActive(true);
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        mainMenu.SetActive(true);
        pauseMenu.SetActive(false);
        inGameUI.SetActive(false);
        Time.timeScale = 0f;
    }
    public void pauseGameMenu()
    {
        mainMenu.SetActive(false);
        pauseMenu.SetActive(true);
        inGameUI.SetActive(false);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        inGameUI.SetActive(true);
        Time.timeScale = 1f;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Hoang");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}