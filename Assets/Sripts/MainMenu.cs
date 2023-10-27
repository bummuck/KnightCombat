using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("here");
        SceneManager.LoadScene("Level1");
    }   

    public void QuitGame()
    {
        Application.Quit();
    }

}
