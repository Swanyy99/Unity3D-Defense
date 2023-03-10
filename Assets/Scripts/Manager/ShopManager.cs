using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : SingleTon<ShopManager>
{

    [SerializeField]
    private ShopUI shop;

    private ShopUnit[] shopUnit;

    public List<InventoryItem> items = new List<InventoryItem>();

    public void AddShopList(InventoryItem item)
    {
        items.Add(item);
        UpdateShopItemList();
    }

    public void UpdateShopItemList()
    {

        shopUnit = shop.GetComponentsInChildren<ShopUnit>();

        for (int i = 0; i < shopUnit.Length; i++)
        {
            if (i < items.Count)
            {
                shopUnit[i].AddItem(items[i]);

            }
        }
    }
}
