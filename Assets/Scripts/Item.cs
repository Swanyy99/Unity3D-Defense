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
        pool = GetComponent<PoolableObject>();
    }

    public void Get()
    {
        InventoryItem inventoryItem = new InventoryItem();
        inventoryItem.data = data;
        //InventoryManager.Instance.NowItem = inventoryItem;
        InventoryManager.Instance.AddItem(inventoryItem);
        LogManager.Instance.logText.text += "<#1E90FF>[¾Ë¸²]</color> <#FFFFFF></color>" + inventoryItem.data.name + "À»(¸¦) È¹µæÇß½À´Ï´Ù.\n";
        LogManager.Instance.StartCoroutine(LogManager.Instance.updateScroll());
        pool.Return();
        //Destroy(gameObject);
    }


    
}
