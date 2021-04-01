using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSelector : MonoBehaviour
{
    public enum Mode
    {
        Tap,
        Timed,
        Slide
    }

    [SerializeField] public static Mode mode = Mode.Slide;

    // Start is called before the first frame update
    void Start()
    {
        SelectMode();
    }

    public void SelectMode(Mode newMode)
    {
        mode = newMode;
        SelectMode();
    }

    public void SelectMode()
    {
        switch (mode)
        {
            case Mode.Tap:
                gameObject.GetComponent<TapController>().enabled = true;
                gameObject.GetComponent<TimedTapController>().enabled = false;
                gameObject.GetComponent<SlideController>().enabled = false;
                break;
            case Mode.Timed:
                gameObject.GetComponent<TapController>().enabled = false;
                gameObject.GetComponent<TimedTapController>().enabled = true;
                gameObject.GetComponent<SlideController>().enabled = false;
                break;
            case Mode.Slide:
                gameObject.GetComponent<TapController>().enabled = false;
                gameObject.GetComponent<TimedTapController>().enabled = false;
                gameObject.GetComponent<SlideController>().enabled = true;
                break;
        }
    }
}
