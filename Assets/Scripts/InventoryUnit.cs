using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUnit : MonoBehaviour
{
    private InventoryItem Item;

    [SerializeField]
    private Button useButton;
    [SerializeField]
    private TextMeshProUGUI textUI;
    [SerializeField]
    private Image icon;


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
    }
}
