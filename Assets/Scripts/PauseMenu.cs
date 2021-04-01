using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject gameGrid;
    bool isPaused = false;

    public void PauseButton()
    {
        if (!isPaused)
        {
            menuPanel.SetActive(true);
            gameGrid.SetActive(false);
            isPaused = !isPaused;
        }
        else
        {
            menuPanel.SetActive(false);
            gameGrid.SetActive(true);
            isPaused = !isPaused;
        }
    }
}
