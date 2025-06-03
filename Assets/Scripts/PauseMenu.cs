using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void ContinueGame()
    {
        gameManager.ResumeGame();
    }
}
