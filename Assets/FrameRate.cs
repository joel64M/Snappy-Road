using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRate : MonoBehaviour
{
    float deltaTime = 0.0f;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }
    int w = Screen.width, h = Screen.height;
    string text;
      float fps;
    void OnGUI()
    {
      

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        // float msec = deltaTime * 1000.0f;
        fps = 1.0f / deltaTime;
        text = string.Format("({0:0} fps)", fps);
        GUI.Label(rect, text, style);
    }
}
