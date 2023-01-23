using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ShopUnit : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image icon;

    public InventoryItem Item;

    [SerializeField]
    private GameObject ItemTooltipUI;

    [SerializeField]
    private ItemTooltipUI tooltip;

    [SerializeField]
    private TextMeshProUGUI ItemName;
    [SerializeField]
    private TextMeshProUGUI ItemType;
    [SerializeField]
    private TextMeshProUGUI ItemDescriprion;
    [SerializeField]
    private TextMeshProUGUI ItemCost;

    void Start()
    {
        
    }



    public void AddItem(InventoryItem item)
    {
        Debug.Log("add 아이템 발동함");
        icon.sprite = item.data.icon;
        icon.color = new Color(255, 255, 255, 255);
        this.Item = item;

    }

    public void RemoveItem()
    {
        this.Item = null;
        icon.sprite = null;
        icon.color = new Color(255, 255, 255, 0);
        this.Item = null;
    }

    public void ShowTooltip()
    {

        if (this.Item != null)
        {
            ItemName.text = this.Item.data.name;
            ItemDescriprion.text = this.Item.data.description;
            ItemCost.text = "구매 가격 : " + this.Item.data.PurchaseCost + " G\n" +
                            "판매 가격 : " + this.Item.data.SellCost + " G";
            ItemType.text = null;
            if (this.Item.data.Itemtype.ToString() == "Potion")
            {
                ItemType.text = "[소모품]";
            }

            else if ((this.Item.data.Itemtype.ToString() == "Material"))
            {
                ItemType.text = "[잡템]";
            }

            else if ((this.Item.data.Itemtype.ToString() == "Equipment"))
            {
                if (this.Item.data.EquipType.ToString() == "Head")
                    ItemType.text = "[모자]";
                if (this.Item.data.EquipType.ToString() == "Armor")
                    ItemType.text = "[상의]";
                if (this.Item.data.EquipType.ToString() == "Pants")
                    ItemType.text = "[하의]";
                if (this.Item.data.EquipType.ToString() == "Glove")
                    ItemType.text = "[장갑]";
                if (this.Item.data.EquipType.ToString() == "Shoes")
                    ItemType.text = "[신발]";
                if (this.Item.data.EquipType.ToString() == "Weapon")
                    ItemType.text = "[무기]";
            }
            else
                ItemType.text = "[???]";

            ItemTooltipUI.SetActive(true);
        }

    }

    public void HideToolTip()
    {
        if (ItemTooltipUI.activeSelf)
            ItemTooltipUI.SetActive(false);
    }

    public void PurchaseItem()
    {
        if (BuildManager.Instance.Gold >= Item.data.PurchaseCost)
        {
            BuildManager.Instance.GoldChange(-Item.data.PurchaseCost);
            InventoryManager.Instance.AddItem(Item);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (this.Item != null)
                PurchaseItem();
        }
    }
}
