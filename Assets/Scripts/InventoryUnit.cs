using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUnit : MonoBehaviour/*, IPointerEnterHandler, IPointerExitHandler*/
{
    private InventoryItem Item;

    [SerializeField]
    private Button useButton;
    //[SerializeField]
    //private TextMeshProUGUI textUI;
    [SerializeField]
    private Image icon;

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
       
    }

    public void RemoveItem()
    {
        useButton.interactable = false;
        icon.sprite = null;
        icon.color = new Color(255, 255, 255, 0);
        this.Item = null;
    }

    public void UseItem()
    {
        Item.Use();
        icon.sprite = null;
        icon.color = new Color(255, 255, 255, 0);
        this.Item = null;
        //Item.ReUpdate();
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
