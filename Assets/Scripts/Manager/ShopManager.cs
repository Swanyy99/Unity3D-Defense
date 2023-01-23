using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : SingleTon<ShopManager>
{

    [SerializeField]
    private ShopUI shop;

    private ShopUnit[] shopUnit;

    public List<InventoryItem> items = new List<InventoryItem>();


    // Start is called before the first frame update
    void Start()
    {
        shopUnit = shop.GetComponentsInChildren<ShopUnit>();
    }
    void Update()
    {
        
    }

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
                Debug.Log("������Ʈ addItem �ߵ�");
                shopUnit[i].AddItem(items[i]);

            }
            else
            {
                Debug.Log("������Ʈ RemoveItem �ߵ�");
                shopUnit[i].RemoveItem();
            }
        }
    }
}
