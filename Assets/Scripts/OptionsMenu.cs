using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] GameObject toggleHints;
    void Start()
    {
        UpdateToggleSwitch();
    }

    public void ChangeHintModeValue()
    {
        //testing
        // string output = "Hint Mode is set to: ";
        // string change = "Hint Mode Changed to: ";
        if (toggleHints.GetComponent<Toggle>().isOn)
        {
            // output += PlayerPrefs.GetInt("HintMode");
            // Debug.Log(output);
            PlayerPrefs.SetInt("HintMode", 1);
            // change += PlayerPrefs.GetInt("HintMode");
            // Debug.Log(change);
        }
        else
        {
            // output += PlayerPrefs.GetInt("HintMode");
            // Debug.Log(output);
            PlayerPrefs.SetInt("HintMode", 0);
            // change += PlayerPrefs.GetInt("HintMode");
            // Debug.Log(change);
        }
    }

    void UpdateToggleSwitch()
    {
        bool value = (PlayerPrefs.GetInt("HintMode") == 1) ? true : false;
        if (value != toggleHints.GetComponent<Toggle>().isOn)
            toggleHints.GetComponent<Toggle>().isOn = value;
    }
}
