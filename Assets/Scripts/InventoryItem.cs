using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public ItemData data;

    public void Use()
    {
        InventoryManager.Instance.UseItem(this);
    }

    public void UseEliminate()
    {
        InventoryManager.Instance.EliminateItem(this);
    }


}
