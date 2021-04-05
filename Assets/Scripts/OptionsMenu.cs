using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{

    public void ChangeHintModeValue() {
        //testing
        string output = "Hint Mode is set to: ";
        string change = "Hint Mode Changed to: ";
        if (PlayerPrefs.GetInt("HintMode") == 0) {
            output += PlayerPrefs.GetInt("HintMode");
            Debug.Log(output);
            PlayerPrefs.SetInt("HintMode", 1);
            change += PlayerPrefs.GetInt("HintMode");
            Debug.Log(change);

        }
        else {
            output += PlayerPrefs.GetInt("HintMode");
            Debug.Log(output);
            PlayerPrefs.SetInt("HintMode", 0);
            change += PlayerPrefs.GetInt("HintMode");
            Debug.Log(change);
        }

        

    }
}
