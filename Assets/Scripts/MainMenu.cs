using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Nam");
    }


    public void Quit()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
