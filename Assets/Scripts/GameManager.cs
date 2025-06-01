using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private int score = 0;
    [SerializeField] private TextMeshProUGUI  scoreText;
    [SerializeField] private GameObject gameOverUI;
    private bool isGameOver = false;
    [SerializeField] private GameObject pauseMenu;
    void Start()
    {
        UpdateScore();
        gameOverUI.SetActive(false);
    }

    // Update is called once per frame
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
        SceneManager.LoadScene("Game");
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void pauseGameMenu()
    {
        pauseMenu.SetActive(true);
        Time.timeScale= 0f ;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }
}
