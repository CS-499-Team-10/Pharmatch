using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuBackground : MonoBehaviour
{
    float alpha = 1.0f;
    [SerializeField] Image background;

    // Update is called once per frame
    void Update()
    {
        alpha -= 1f * (float)Time.deltaTime;
        if (alpha <= 0f) Destroy(this.gameObject);
        background.color = new Color(1.0f, 1.0f, 1.0f, alpha);
    }
}
