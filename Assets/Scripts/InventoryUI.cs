using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private InventoryUnit[] inventoryUnits;

    //private InventoryItem item;

    public void UpdateUI()
    {
        inventoryUnits = GetComponentsInChildren<InventoryUnit>();

        for (int i = 0; i < inventoryUnits.Length; i++)
        {

            if (i < InventoryManager.Instance.items.Count)
            {
                inventoryUnits[i].AddItem(InventoryManager.Instance.items[i]);
            }

            else
            {
                inventoryUnits[i].RemoveItem();
            }
            
        }
    }
}
