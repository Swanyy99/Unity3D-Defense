using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private ItemData data;

    public void Get()
    {
        InventoryItem inventoryItem = new InventoryItem();
        inventoryItem.data = data;
        //InventoryManager.Instance.NowItem = inventoryItem;
        InventoryManager.Instance.AddItem(inventoryItem);

        Destroy(gameObject);
    }
}
