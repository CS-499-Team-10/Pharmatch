using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject gameGrid;
    static bool _isPaused = false;
    public static bool isPaused
    {
        get { return _isPaused; }
    }

    private void Start()
    {
        _isPaused = false;
        Time.timeScale = 1;
        menuPanel.SetActive(false);
        gameGrid.SetActive(true);
    }

    public void PauseButton()
    {
        if (!_isPaused)
        {
            Time.timeScale = 0;
            menuPanel.SetActive(true);
            gameGrid.SetActive(false);
            _isPaused = !_isPaused;
        }
        else
        {
            Time.timeScale = 1;
            menuPanel.SetActive(false);
            gameGrid.SetActive(true);
            _isPaused = !_isPaused;
        }
    }

    public void Quit()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
