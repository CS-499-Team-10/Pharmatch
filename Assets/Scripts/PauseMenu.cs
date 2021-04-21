using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject gameGrid;
    GameObject backgroundMusic;
    [SerializeField] GameObject soundEffect;
    [SerializeField] GameObject toggleBGM;
    [SerializeField] GameObject toggleSFX;
    static bool _isPaused = false;
    public static bool isPaused
    {
        get { return _isPaused; }
    }
    GameObject FindBGM()
    {
        List<GameObject> objectsInScene = new List<GameObject>();

        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (go.name == "BGM")
            {
                return go;
            }
        }
        return null;
    }

    private void Start()
    {
        _isPaused = false;
        Time.timeScale = 1;
        menuPanel.SetActive(false);
        gameGrid.SetActive(true);
    }

    void Awake()
    {
        backgroundMusic = FindBGM();
        try
        {
            toggleBGM.GetComponent<Toggle>().isOn = backgroundMusic.activeInHierarchy;
            toggleSFX.GetComponent<Toggle>().isOn = soundEffect.activeInHierarchy;
        }
        catch (System.Exception)
        {
        }

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

    public void ToggleSFX()
    {
        soundEffect.SetActive(toggleSFX.GetComponent<Toggle>().isOn);
    }

    public void ToggleMusic()
    {
        // Debug.Log("turning musing to: " + value);
        backgroundMusic.SetActive(toggleBGM.GetComponent<Toggle>().isOn);
    }
}
