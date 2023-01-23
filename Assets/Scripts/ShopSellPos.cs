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
        LogManager.Instance.logText.text += "<#32CD32>[�˸�]</color><#FFD700> " + DragSlot.instance.dragSlot.Item.data.name + " </color><#FFFFFF></color>��(��) �Ǹ��߽��ϴ�.\n";
        LogManager.Instance.StartCoroutine(LogManager.Instance.updateScroll());
        DragSlot.instance.dragSlot.RemoveItem();
    }

}
