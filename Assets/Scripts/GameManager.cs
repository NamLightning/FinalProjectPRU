using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private int score = 0;
    [SerializeField] private TextMeshProUGUI  scoreText;
    [SerializeField] private GameObject gameOverUI;
    private bool isGameOver = false;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject pauseMenu;

    void Start()
    {
        Time.timeScale = 1f;
        UpdateScore();
        gameOverUI.SetActive(false);
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
        isGameOver = true;
        score = 0;
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
    }

    public void RestartGame()
    {
        isGameOver=false;
        score = 0;
        UpdateScore() ;
        Time.timeScale = 1;
        SceneManager.LoadScene("Hoang");
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
