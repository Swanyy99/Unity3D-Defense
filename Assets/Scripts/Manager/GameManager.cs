using Cinemachine;
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
    public bool TooltipOn;
    public bool HelpConversation;
    public bool isPaused;

    [Header("UI")]
    public GameObject TowerPlaceUIOpenButton;
    public GameObject nowTowerImage;
    public GameObject TowerPlaceUI;
    public GameObject StartButtonUI;
    public GameObject BuildModeUI;
    public GameObject BlackCover;

    [Header("PopupUI")]
    [SerializeField]
    private GameObject EquipUI;
    [SerializeField]
    private GameObject InvenUI;
    [SerializeField]
    private GameObject ShopUI;

    [SerializeField]
    private CinemachineFreeLook playerCam;

    public Enemy target;

    private void Start()
    {
        BlackCover.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && WaveManager.Instance.SpawnedMonster == 0)
            GameOn = true;

        if (Input.GetKeyDown(KeyCode.F1) && TooltipOn == true)
        {
            TooltipOn = false;
        }
        else if (Input.GetKeyDown(KeyCode.F1) && TooltipOn == false)
        {
            TooltipOn = true;
        }

        if (BuildMode == false)
        {
            nowTowerImage.SetActive(false);
            TowerPlaceUI.SetActive(false);
            StartButtonUI.SetActive(false);
            BuildModeUI.SetActive(false);
        }
        else if (BuildMode == true) 
        {
            nowTowerImage.SetActive(true);
            TowerPlaceUI.SetActive(true);
            StartButtonUI.SetActive(true);
            BuildModeUI.SetActive(true);

            if (Input.GetMouseButton(1))
            {
                nowTowerImage.SetActive(false);
                //TowerPlaceUI.SetActive(false);
                //StartButtonUI.SetActive(false);
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