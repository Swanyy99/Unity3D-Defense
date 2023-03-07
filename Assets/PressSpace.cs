using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PressSpace : MonoBehaviour
{
    [SerializeField]
    private GameObject blackCover;

    private void Start()
    {
        //SetWindowSize();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            blackCover.SetActive(true);
        }
    }

    private void SetWindowSize()
    {
        int Width = 1600;
        int height = 900;

        Screen.SetResolution(Width, height, false);
    }


}
