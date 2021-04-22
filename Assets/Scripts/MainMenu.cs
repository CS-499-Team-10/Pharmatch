using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] public AudioClip tap;
    [SerializeField] public AudioClip timed;
    [SerializeField] public AudioClip slide;

    private GameObject music;

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

    public void Awake()
    {
        music = FindBGM();
        if(music != null && music.GetComponent<AudioSource>().clip != tap)
        {
            music.GetComponent<AudioSource>().clip = tap;
            music.GetComponent<AudioSource>().Play();
        }
    }

    public void PlayTapGame()
    {
        ModeSelector.mode = ModeSelector.Mode.Tap; // set a certain game mode
        if (music != null && music.GetComponent<AudioSource>().clip != tap)
        {
            music.GetComponent<AudioSource>().clip = tap;
            music.GetComponent<AudioSource>().Play();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void PlayTimedGame()
    {
        ModeSelector.mode = ModeSelector.Mode.Timed; // set a certain game mode
        if (music != null && music.GetComponent<AudioSource>().clip != timed)
        {
            music.GetComponent<AudioSource>().clip = timed;
            music.GetComponent<AudioSource>().Play();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void PlaySlideGame()
    {
        ModeSelector.mode = ModeSelector.Mode.Slide; // set a certain game mode
        if (music != null && music.GetComponent<AudioSource>().clip != slide)
        {
            music.GetComponent<AudioSource>().clip = slide;
            music.GetComponent<AudioSource>().Play();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
