using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{

    public void LoadStartScene()
    {
        SceneManager.LoadScene("StartScreen");
    }

    public void LoadInstructionsScene()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
