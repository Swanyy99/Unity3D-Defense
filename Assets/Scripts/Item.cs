using ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField]
    private ItemData data;

    private PoolableObject pool;

    private void Start()
    {
    }

    public void Get()
    {
        pool = GetComponent<PoolableObject>();

        InventoryItem inventoryItem = new InventoryItem();
        inventoryItem.data = data;
        //InventoryManager.Instance.NowItem = inventoryItem;
        InventoryManager.Instance.AddItem(inventoryItem);
        LogManager.Instance.logText.text += "<#1E90FF>[¾Ë¸²]</color> <#FFFFFF></color>" + inventoryItem.data.name + "À»(¸¦) È¹µæÇß½À´Ï´Ù.\n";
        LogManager.Instance.StartCoroutine(LogManager.Instance.updateScroll());
        pool.Return();
        //Destroy(gameObject);
    }

    public void Acquire()
    {
        InventoryItem inventoryItem = new InventoryItem();
        inventoryItem.data = data;
        //InventoryManager.Instance.NowItem = inventoryItem;
        InventoryManager.Instance.AddItem(inventoryItem);
        LogManager.Instance.logText.text += "<#1E90FF>[¾Ë¸²]</color> <#FFFFFF></color>" + inventoryItem.data.name + "À»(¸¦) È¹µæÇß½À´Ï´Ù.\n";
        LogManager.Instance.StartCoroutine(LogManager.Instance.updateScroll());
        Destroy(gameObject);
    }

    public void AddShop()
    {
        InventoryItem inventoryItem = new InventoryItem();
        inventoryItem.data = data;
        ShopManager.Instance.AddShopList(inventoryItem);
        //InventoryManager.Instance.NowItem = inventoryItem;
        //InventoryManager.Instance.AddItem(inventoryItem);
        //LogManager.Instance.logText.text += "<#1E90FF>[¾Ë¸²]</color> <#FFFFFF></color>" + inventoryItem.data.name + "À»(¸¦) È¹µæÇß½À´Ï´Ù.\n";
        //LogManager.Instance.StartCoroutine(LogManager.Instance.updateScroll());
    }



}
