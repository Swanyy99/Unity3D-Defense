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

            case "Book":
                Debug.Log("스탯북을 사용했다");
                UseStatBook(inventoryItem);
                break;

            default:
                break;

        }

    }

    public void EquipUIShow()
    {
        EquipmentUI.gameObject.SetActive(true);
    }

    public void ShowInven()
    {
        Inventory.gameObject.SetActive(true);
    }


    public void UseStatBook(InventoryItem inventoryItem)
    {
        PlayerManager.Instance.originSTR += inventoryItem.data.Upgrade_STR_STAT;
        PlayerManager.Instance.originDEF += inventoryItem.data.Upgrade_DEF_STAT;
        PlayerManager.Instance.originDEX += inventoryItem.data.Upgrade_DEX_STAT;
        PlayerManager.Instance.originINT += inventoryItem.data.Upgrade_INT_STAT;
        PlayerManager.Instance.originMAXHP += inventoryItem.data.Upgrade_MHP_STAT;
        PlayerManager.Instance.originMAXMP += inventoryItem.data.Upgrade_MMP_STAT;
        PlayerManager.Instance.originHPR += inventoryItem.data.Upgrade_HPR_STAT;
        PlayerManager.Instance.originMPR += inventoryItem.data.Upgrade_MPR_STAT;
        PlayerManager.Instance.StatUpdate();
        Instantiate(inventoryItem.data.UseEffect, player.transform.position, player.transform.rotation);
    }



}
