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


    public int ItemCount = 1;

    private InventoryUI inven;

    [SerializeField]
    private GameObject ItemTooltipUI;

    [SerializeField]
    private TextMeshProUGUI ItemName;
    [SerializeField]
    private TextMeshProUGUI ItemDescriprion;

    public void AddItem(InventoryItem inventoryItem/*, int val*/)
    {
        Debug.Log("add 발동");
        useButton.interactable = true;
        icon.sprite = inventoryItem.data.icon;
        icon.color = new Color(255, 255, 255, 255);
        this.Item = inventoryItem;
        //ItemCount = val;
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
        Debug.Log("remove 발동");
        icon.sprite = null;
        icon.color = new Color(255, 255, 255, 0);
        count.text = "";
        this.Item = null;
        useButton.interactable = false;
    }

    public void UseItem()
    {
        if (ItemCount > 1)
        {
            Item.Use();
            SetItemCount(Item, -1);
            Debug.Log("아이템 갯수가 2개이상이라 발동");
        }

        else if (ItemCount == 1)
        {
            Item.UseEliminate();
            icon.sprite = null;
            icon.color = new Color(255, 255, 255, 0);
            count.text = "";
            useButton.interactable = false;
            this.Item = null;
            Debug.Log("아이템 갯수가 1개라 발동");
        }

    }


    public string GetItemName()
    {
        return this.Item.data.name;
    }

    public void ShowTooltip()
    {
        if (this.Item != null)
        {
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
