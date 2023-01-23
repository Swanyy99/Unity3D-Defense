using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopSellPos : MonoBehaviour, IDropHandler
{
    
    public void OnDrop(PointerEventData eventData)
    {
        SellItem();
    }

    private void SellItem()
    {
        BuildManager.Instance.GoldChange(DragSlot.instance.dragSlot.Item.data.SellCost * DragSlot.instance.dragSlot.ItemCount);
        DragSlot.instance.dragSlot.RemoveItem();
    }

}
