using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public ItemData data;

    public virtual void Use()
    {
        InventoryManager.Instance.DropItem(this);
    }
}
