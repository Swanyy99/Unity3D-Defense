using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : SingleTon<InventoryManager>
{
    [SerializeField]
    private InventoryUI Inventory;

    [SerializeField]
    private EquipmentUI EquipmentUI;

    [SerializeField]
    private GameObject ShopUI;


    [SerializeField]
    private GameObject TooltipUI;

    //public List<InventoryItem> items = new List<InventoryItem>();

    private GameObject player;

    public GameObject UseEffectPos;

    public bool InventoryOn;

    public bool UI_ON;

    [SerializeField]
    private CinemachineFreeLook playerCam;

    [SerializeField]
    private InventoryUnit[] inven;


    public InventoryItem NowItem;


    private void Start()
    {
        player = GameObject.Find("Player");
        inven = Inventory.GetComponentsInChildren<InventoryUnit>();
    }


    private void Update()
    {
        //if (Inventory.gameObject.activeSelf || EquipmentUI.gameObject.activeSelf)
        //    UI_ON = true;
        //else
        //    UI_ON = false;

        if (GameManager.Instance.BuildMode == false)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (Inventory.gameObject.activeSelf)
                {
                    Inventory.gameObject.SetActive(false);
                    TooltipUI.SetActive(false);
                    ShopUI.SetActive(false);
                    //InventoryOn = false;
                    DetectUION();
                    //if (!EquipmentUI.gameObject.activeSelf)
                    //    UI_ON = false;


                }
                else
                {
                    Inventory.gameObject.SetActive(true);
                    //InventoryOn = true;
                    UI_ON = true;

                }

                if (UI_ON == true || GameManager.Instance.BuildMode == true)
                {
                    playerCam.m_XAxis.m_MaxSpeed = 0;
                    playerCam.m_YAxis.m_MaxSpeed = 0;
                    Cursor.lockState = CursorLockMode.Confined;
                }
                else if (UI_ON == false || GameManager.Instance.BuildMode == false)
                {
                    playerCam.m_XAxis.m_MaxSpeed = 200;
                    playerCam.m_YAxis.m_MaxSpeed = 2;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                if (EquipmentUI.gameObject.activeSelf)
                {
                    EquipmentUI.gameObject.SetActive(false);
                    InventoryOn = false;
                    //UI_ON = false;
                    DetectUION();
                    //if (!Inventory.gameObject.activeSelf)
                    //    UI_ON = false;

                }
                else
                {
                    EquipmentUI.gameObject.SetActive(true);
                    //InventoryOn = true;
                    UI_ON = true;

                }

                if (UI_ON == true || GameManager.Instance.BuildMode == true)
                {
                    playerCam.m_XAxis.m_MaxSpeed = 0;
                    playerCam.m_YAxis.m_MaxSpeed = 0;
                    Cursor.lockState = CursorLockMode.Confined;
                }
                else if (UI_ON == false || GameManager.Instance.BuildMode == false)
                {
                    playerCam.m_XAxis.m_MaxSpeed = 200;
                    playerCam.m_YAxis.m_MaxSpeed = 2;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }


            
            if (Cursor.lockState == CursorLockMode.Locked && Input.GetMouseButtonDown(1))
            {
                Cursor.lockState = CursorLockMode.Confined;
                playerCam.m_XAxis.m_MaxSpeed = 0;
                playerCam.m_YAxis.m_MaxSpeed = 0;
            }
            else if (Cursor.lockState == CursorLockMode.Confined && UI_ON == false && Input.GetMouseButtonDown(1))
            {
                Cursor.lockState = CursorLockMode.Locked;
                playerCam.m_XAxis.m_MaxSpeed = 200;
                playerCam.m_YAxis.m_MaxSpeed = 2;
            }

        }
    }


    public void AddItem(InventoryItem inventoryItem)
    {

        for (int i = 0; i < inven.Length; i++)
        {
            if (inven[i].Item != null)
            {
                if (inventoryItem.data.Itemtype.ToString() != "Equipment")
                {
                    if (inven[i].Item.data.name == inventoryItem.data.name)
                    {
                        inven[i].SetItemCount(1);
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < inven.Length; i++)
        {
            if (inven[i].Item == null)
            {
                inven[i].AddItem(inventoryItem);
                inven[i].SetCount(1);
                return;
            }
        }


        


    }

    public void DropItem(InventoryItem inventoryItem)
    {

        Instantiate(inventoryItem.data.prefab, player.transform.position, player.transform.rotation);
    }

    public void UseItem(InventoryItem inventoryItem)
    {
        string Type = inventoryItem.data.Itemtype.ToString();

        switch (Type)
        {
            case "Potion":
                Debug.Log("포션이 발동한다");
                PlayerManager.Instance.GainHp(inventoryItem.data.RecoverHp);
                PlayerManager.Instance.GainMp(inventoryItem.data.RecoverMp);
                Instantiate(inventoryItem.data.UseEffect, UseEffectPos.transform.position, UseEffectPos.transform.rotation);
                break;

            case "Equipment":
                Debug.Log("검을 분해시켰다");
                break;

            case "Material":
                Debug.Log("재료템을 판매했다");
                BuildManager.Instance.gold += inventoryItem.data.SellCost;
                BuildManager.Instance.GoldUpdate();
                break;

            default:
                break;

        }

    }

    public void EliminateItem(InventoryItem inventoryItem)
    {
        string Type = inventoryItem.data.Itemtype.ToString();

        switch (Type)
        {
            case "Potion":
                Debug.Log(inventoryItem.data.name.ToString() + "이 사용되며, 파괴시킵니다. ");
                PlayerManager.Instance.GainHp(inventoryItem.data.RecoverHp);
                PlayerManager.Instance.GainMp(inventoryItem.data.RecoverMp);
                Instantiate(inventoryItem.data.UseEffect, UseEffectPos.transform.position, UseEffectPos.transform.rotation);
                break;

            case "Equipment":
                Debug.Log(inventoryItem.data.name.ToString() + "이 사용되며, 파괴시킵니다. ");
                break;

            case "Material":
                Debug.Log("재료템을 판매했다");
                BuildManager.Instance.gold += inventoryItem.data.SellCost;
                BuildManager.Instance.GoldUpdate();
                break;

            default:
                break;

        }

        //for (int i = 0; i < inven.Length; i++)
        //{
        //    if (inven[i].Item == null)
        //    {
        //        inven[i].RemoveItem();
        //    }
        //}
    }

    public void EquipUIShow()
    {
        EquipmentUI.gameObject.SetActive(true);
    }

    public void EquipUIHide()
    {
        EquipmentUI.gameObject.SetActive(false);
    }

    private IEnumerator WaitUpdate()
    {
        yield return new WaitForSeconds (0.1f);

    }

    public void ShowUI()
    {
        EquipmentUI.gameObject.SetActive(true);
        Inventory.gameObject.SetActive(true);
    }

    public void ShowInven()
    {
        Inventory.gameObject.SetActive(true);
    }

    public void HideUI()
    {
        EquipmentUI.gameObject.SetActive(false);
        Inventory.gameObject.SetActive(false);
    }

    public void DetectUION()
    {
        if (Inventory.gameObject.activeSelf == false && EquipmentUI.gameObject.activeSelf == false)
            UI_ON = false;

        if (UI_ON == false && GameManager.Instance.BuildMode == false)
        {
            playerCam.m_XAxis.m_MaxSpeed = 200;
            playerCam.m_YAxis.m_MaxSpeed = 2;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }



}
