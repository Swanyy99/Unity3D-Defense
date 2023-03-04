using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class UIManager : SingleTon<UIManager>
{
    [SerializeField]
    private Canvas windowCanvas;

    [SerializeField]
    private Inventory inventoryPrefab;

    //private List<Inventory> inventoryList;
    public bool InventoryOn;

    public GameObject EquipmentUI;

    public GameObject InventoryUI;

    public GameObject ShopUI;

    public GameObject ItemTooltipUI;

    [SerializeField]
    private CinemachineFreeLook playerCam;


    private void Update()
    {
        InputDetect();
    }

    private void InputDetect()
    {
        if (Input.GetKeyDown(KeyCode.I))
            SetActiveUI("Inventory");

        if (Input.GetKeyDown(KeyCode.U))
            SetActiveUI("Equipment");

        if (Input.GetKeyDown(KeyCode.Escape))
            SetActiveUI("");

        if (Input.GetKeyDown(KeyCode.F))
            BuildModeAccess();

        if (Input.GetMouseButtonDown(1))
            UISelectMode();

    }

    public void SetActiveUI(string ui)
    {
        if (GameManager.Instance.BuildMode == true) return;

        bool val;

        if (ui == "Equipment")
        {
            val = EquipmentUI.activeSelf ? true : false;
            EquipmentUI.SetActive(!val);
        }

        if (ui == "Inventory")
        {
            val = InventoryUI.activeSelf ? true : false;
            InventoryUI.SetActive(!val);
        }

        DecideBehaviour(ui);
        UI_On();
        SetMouse();
    }

    private void BuildModeAccess()
    {
        if (Input.GetKeyDown(KeyCode.F) && GameManager.Instance.BuildMode == true)
        {
            GameManager.Instance.BuildMode = false;
            GameManager.Instance.TooltipOn = false;
            if (UI_On() == false)
                Cursor.lockState = CursorLockMode.Locked;
        }
        else if (Input.GetKeyDown(KeyCode.F) && GameManager.Instance.BuildMode == false)
        {
            HideUI();
            Cursor.lockState = CursorLockMode.Confined;
            GameManager.Instance.BuildMode = true;
            GameManager.Instance.TooltipOn = true;
        }
    }

    private void DecideBehaviour(string ui)
    {
        switch (ui)
        {
            case "Equipment":
                playerCam.m_XAxis.m_MaxSpeed = 0;
                playerCam.m_YAxis.m_MaxSpeed = 0;
                Cursor.lockState = CursorLockMode.Confined;
                break;

            case "Inventory":
                playerCam.m_XAxis.m_MaxSpeed = 0;
                playerCam.m_YAxis.m_MaxSpeed = 0;
                Cursor.lockState = CursorLockMode.Confined;
                break;

            case "":
                this.ShopUI.SetActive(false);
                playerCam.m_XAxis.m_MaxSpeed = 200;
                playerCam.m_YAxis.m_MaxSpeed = 2;
                Cursor.lockState = CursorLockMode.Locked;
                break;

            default:    break;
        }
    }

    public bool UI_On()
    {
        bool result;
        result = EquipmentUI.activeSelf || InventoryUI.activeSelf || ShopUI.activeSelf;
        return result;
    }

    public void UISelectMode()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Confined;
            playerCam.m_XAxis.m_MaxSpeed = 0;
            playerCam.m_YAxis.m_MaxSpeed = 0;
        }
        else if (Cursor.lockState == CursorLockMode.Confined && UI_On() == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            playerCam.m_XAxis.m_MaxSpeed = 200;
            playerCam.m_YAxis.m_MaxSpeed = 2;
        }
    }

    public void SetMouse()
    {
        if (UI_On()) 
        {
            Cursor.lockState = CursorLockMode.Confined;
            playerCam.m_XAxis.m_MaxSpeed = 0;
            playerCam.m_YAxis.m_MaxSpeed = 0;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            playerCam.m_XAxis.m_MaxSpeed = 200;
            playerCam.m_YAxis.m_MaxSpeed = 2;
            HideUI();
        }
    }

    public void HideUI()
    {
        EquipmentUI.gameObject.SetActive(false);
        InventoryUI.gameObject.SetActive(false);
        ItemTooltipUI.gameObject.SetActive(false);
    }

}
