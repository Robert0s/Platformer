using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private int gameSceneIndex = 1;

    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneIndex);
    }

    public void QuitGame()
    {
        
        Application.Quit();
        Debug.Log("Quit!");
    }
}