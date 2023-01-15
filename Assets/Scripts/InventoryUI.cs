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
                //if (inventoryUnits[i].Item == null)
                //else if (inventoryUnits[i].Item != null && inventoryUnits[i].Item.data.name != InventoryManager.Instance.items[i].data.name)
                    //inventoryUnits[i].SetItemCount(InventoryManager.Instance.items[i], 1);

            }
            else
            {
                inventoryUnits[i].RemoveItem();
            }
        }
    }
}
