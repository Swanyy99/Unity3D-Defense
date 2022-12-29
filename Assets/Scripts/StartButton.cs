using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    [SerializeField]
    private Button startButton;


    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
    }

    public void StartGame()
    {
        if (GameManager.Instance.GameOn == true)
            return;

        if (GameManager.Instance.GameOn == false)
        {
            GameManager.Instance.GameOn = true;

        }
    }


}