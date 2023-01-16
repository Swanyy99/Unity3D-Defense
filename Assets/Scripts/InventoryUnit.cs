using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryUnit : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler /*, IPointerEnterHandler, IPointerExitHandler*/
{
    public InventoryItem Item;

    [SerializeField]
    private Button useButton;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Image dragIcon;

    [SerializeField]
    private TextMeshProUGUI count;

    public int ItemCount = 0;

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
    }

    public void SetItemCount(InventoryItem inventoryItem, int num)
    {
        ItemCount += num;
        count.text = ItemCount.ToString();
        if (count.text == "1")
            count.text = "";
    }

    public void SetCount(int num)
    {
        ItemCount = num;
        count.text = ItemCount.ToString();
        if (count.text == "1")
            count.text = "";
    }

    public void RemoveItem()
    {
        icon.sprite = null;
        icon.color = new Color(255, 255, 255, 0);
        count.text = "";
        this.Item = null;
        ItemCount = 0;
    }

    public void UseItem()
    {
        if (this.Item != null)
        {
            if (ItemCount > 1)
            {
                Item.Use();
                SetItemCount(Item, -1);
                Debug.Log("������ ������ 2���̻��϶� ����߽��ϴ�.");
            }

            else if (ItemCount == 1)
            {
                Item.UseEliminate();
                icon.sprite = null;
                icon.color = new Color(255, 255, 255, 0);
                count.text = "";
                this.Item = null;
                Debug.Log("������ ������ 1���϶� ����߽��ϴ�.");
            }
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

    public void IsThereITEM()
    {
        if (this.Item == null)
            HideToolTip();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) 
        {
            if (this.Item != null)
                UseItem();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (this.Item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(this.icon);
            DragSlot.instance.transform.position = eventData.position;
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (this.Item != null)
            DragSlot.instance.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
        ItemTooltipUI.gameObject.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
            ChangeSlot();
    }

    private void ChangeSlot()
    {
        InventoryItem _tempItem = this.Item;
        int _tempItemCount = ItemCount;

        AddItem(DragSlot.instance.dragSlot.Item);
        SetCount(DragSlot.instance.dragSlot.ItemCount);

        if (_tempItem != null)
        {
            DragSlot.instance.dragSlot.AddItem(_tempItem);
            DragSlot.instance.dragSlot.SetCount(_tempItemCount);
        }
        else
        {
            DragSlot.instance.dragSlot.RemoveItem();
        }
    }
}
