using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : SingleTon<GameManager>
{
    [Header("Game")]
    public float GameSpeed = 1f;
    public bool GameOn;
    public bool BuildMode;

    [Header("UI")]
    public GameObject nowTowerImage;
    public GameObject TowerPlaceUI;
    public GameObject StartButtonUI;

    public Enemy target;

    

    private void Update()
    {
        if (BuildMode == false)
        {
            nowTowerImage.SetActive(false);
            TowerPlaceUI.SetActive(false);
            StartButtonUI.SetActive(false);
        }
        else if (BuildMode == true) 
        {
            nowTowerImage.SetActive(true);
            TowerPlaceUI.SetActive(true);
            StartButtonUI.SetActive(true);

            if (Input.GetMouseButton(1))
            {
                nowTowerImage.SetActive(false);
                TowerPlaceUI.SetActive(false);
                StartButtonUI.SetActive(false);
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = GameSpeed;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}