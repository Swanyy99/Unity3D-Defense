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
    private GameObject TooltipUI;

    //public List<InventoryItem> items = new List<InventoryItem>();

    private GameObject player;

    public GameObject UseEffectPos;

    public bool InventoryOn;

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

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (Inventory.gameObject.activeSelf)
            {
                Inventory.gameObject.SetActive(false);
                TooltipUI.SetActive(false);
                InventoryOn = false;
                
            }
            else
            {
                Inventory.gameObject.SetActive(true);
                InventoryOn = true;
                
            }
        }

        if (Inventory.gameObject.activeSelf == true || GameManager.Instance.BuildMode == true)
        {
            playerCam.m_XAxis.m_MaxSpeed = 0;
            playerCam.m_YAxis.m_MaxSpeed = 0;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else if (Inventory.gameObject.activeSelf == false || GameManager.Instance.BuildMode == false)
        {
            playerCam.m_XAxis.m_MaxSpeed = 200;
            playerCam.m_YAxis.m_MaxSpeed = 2;
            Cursor.lockState = CursorLockMode.Locked;
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

    private IEnumerator WaitUpdate()
    {
        yield return new WaitForSeconds (0.1f);

    }



}
