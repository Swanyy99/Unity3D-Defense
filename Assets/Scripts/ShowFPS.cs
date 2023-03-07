using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowFPS : MonoBehaviour
{
    public Font font;
    [Range(10, 150)]
    public int fontSize = 30;
    public Color color = new Color(.0f, .0f, .0f, 1.0f);
    public float width, height;

    private bool FPS_On;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) FPS_On = !FPS_On;    
    }

    void OnGUI()
    {
        if (!FPS_On) return;

        Rect position = new Rect(width, height, Screen.width, Screen.height);

        float fps = 1.0f / Time.deltaTime;
        string text = string.Format("{0:N0} FPS", fps);

        GUIStyle style = new GUIStyle();

        style.fontSize = fontSize;
        style.normal.textColor = color;
        style.font = font;

        GUI.Label(position, text, style);
    }
}
