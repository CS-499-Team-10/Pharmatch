// https://answers.unity.com/questions/11314/audio-or-music-to-continue-playing-between-scene-c.html

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUnitySingleton : MonoBehaviour
{
    private static MyUnitySingleton instance = null;
    public static MyUnitySingleton Instance
    {
        get { return instance; }
    }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
