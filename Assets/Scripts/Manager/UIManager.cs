using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingleTon<UIManager>
{
    [SerializeField]
    private Canvas windowCanvas;

    [SerializeField]
    private Inventory inventoryPrefab;

    private List<Inventory> inventoryList;

    public bool InventoryOn;


    private void Awake()
    {
        inventoryList = new List<Inventory>();
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.I))
        //    CreateInventory();
    }

    public void CreateInventory()
    {
        if (InventoryOn == false)
        {

            Inventory inventory = Instantiate(inventoryPrefab);
            inventory.transform.SetParent(windowCanvas.transform, false);
            InventoryOn = true;
        }
        
        //inventoryList.Add(inventory);
    }

    //public void SetFocusWindow(Inventory inventory)
    //{
    //    inventoryList.Remove(inventory);
    //    inventoryList.Add(inventory);

    //    for (int i = 0; i <inventoryList.Count; i++) 
    //    {
    //        //inventoryList[i].
    //    }
    //}
}
