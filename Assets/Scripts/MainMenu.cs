using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void PlayTapGame()
    {
        ModeSelector.mode = ModeSelector.Mode.Tap; // set a certain game mode
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void PlayTimedGame()
    {
        ModeSelector.mode = ModeSelector.Mode.Timed; // set a certain game mode
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void PlaySlideGame()
    {
        ModeSelector.mode = ModeSelector.Mode.Slide; // set a certain game mode
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
