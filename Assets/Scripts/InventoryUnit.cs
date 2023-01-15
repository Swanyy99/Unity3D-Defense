using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUnit : MonoBehaviour/*, IPointerEnterHandler, IPointerExitHandler*/
{
    public InventoryItem Item;

    [SerializeField]
    private Button useButton;
    //[SerializeField]
    //private TextMeshProUGUI textUI;
    [SerializeField]
    private Image icon;

    [SerializeField]
    private TextMeshProUGUI count;


    private int ItemCount = 1;

    private InventoryUI inven;

    [SerializeField]
    private GameObject ItemTooltipUI;

    [SerializeField]
    private TextMeshProUGUI ItemName;
    [SerializeField]
    private TextMeshProUGUI ItemDescriprion;

    public void AddItem(InventoryItem inventoryItem)
    {
        useButton.interactable = true;
        icon.sprite = inventoryItem.data.icon;
        icon.color = new Color(255, 255, 255, 255);
        this.Item = inventoryItem;

        //if(this.Item != null)
        //    Debug.Log("먹기 전 아이템 " + this.Item.data.name);

        //Debug.Log("들어올 아이템 " + inventoryItem.data.name);

        //if (this.Item != null)
        //{
        //    if (this.Item.data.name == inventoryItem.data.name)
        //    {
        //        SetItemCount(inventoryItem, 1);
        //        return;
        //    }
        //}

        //if (this.Item == null)
        //{
        //    SetItemCount(inventoryItem, 1);
        //}

        //else if (this.Item != null && this.Item.data.name == inventoryItem.data.name)
        //{
        //    SetItemCount(inventoryItem, 1);
        //}

        //Debug.Log("획득 후 아이템 " + this.Item.data.name);



    }

    public void SetItemCount(InventoryItem inventoryItem, int num)
    {
        ItemCount += num;
        count.text = ItemCount.ToString();
        if (count.text == "1")
            count.text = "";
    }

    public void RemoveItem()
    {
        this.Item = null;
        useButton.interactable = false;
        icon.sprite = null;
        icon.color = new Color(255, 255, 255, 0);
        count.text = "";
    }

    public void UseItem()
    {
        if (ItemCount > 1)
        {
            Item.Use();
            SetItemCount(Item, -1);
        }

        else
        {
            Item.UseEliminate();
            icon.sprite = null;
            icon.color = new Color(255, 255, 255, 0);
            this.Item = null;
            count.text = "";
            useButton.interactable = false;
        }
        
        //Item.ReUpdate();
    }


    public string GetItemName()
    {
        return this.Item.data.name;
    }

    public void ShowTooltip()
    {
        if (this.Item != null)
        {
            Debug.Log("마우스올라감");
            ItemTooltipUI.SetActive(true);
            ItemName.text = this.Item.data.name;
            ItemDescriprion.text = this.Item.data.description;
        }
    }

    public void HideToolTip()
    {
        if (ItemTooltipUI.activeSelf)
            ItemTooltipUI.SetActive(false);
    }




}
