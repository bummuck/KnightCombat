using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    [Header("PausedGame")]
    public GameObject pauseMenuUI;
    public GameObject gameOverMenuUI;
    public GameObject finishedMenuUI;
    public static bool pausedGame = false;
    public GameObject hints;

    private bool isHintVisible = false;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausedGame)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            ShowHideHint();
        }
    }

    public void GameOver()
    {
        gameOverMenuUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void Finished()
    {
        Time.timeScale = 0;
        finishedMenuUI.SetActive(true);
    }

    public void Restart()
    {
        gameOverMenuUI.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNext()
    {
        finishedMenuUI.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        pausedGame = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        pausedGame = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowHideHint()
    {
        isHintVisible = !isHintVisible;
        hints.SetActive(isHintVisible);
    }
}
