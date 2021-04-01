using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject gameGrid;
    static bool _isPaused = false;
    public static bool isPaused
    {
        get { return _isPaused; }
    }

    public void PauseButton()
    {
        if (!_isPaused)
        {
            menuPanel.SetActive(true);
            gameGrid.SetActive(false);
            _isPaused = !_isPaused;
        }
        else
        {
            menuPanel.SetActive(false);
            gameGrid.SetActive(true);
            _isPaused = !_isPaused;
        }
    }
}
