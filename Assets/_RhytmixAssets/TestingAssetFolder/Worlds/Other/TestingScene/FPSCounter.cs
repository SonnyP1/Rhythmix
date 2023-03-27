using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    Text text;
    private void Start()
    {
        text = GetComponent<Text>();
    }
    void Update()
    {
        int currentFPS = (int)(1f / Time.unscaledDeltaTime);
        text.text = "FPS: " + currentFPS.ToString();
    }
}
