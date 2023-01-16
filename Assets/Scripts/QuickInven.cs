using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickInven : MonoBehaviour
{
    [SerializeField]
    private InventoryUnit quickSlot1;

    [SerializeField]
    private InventoryUnit quickSlot2;

    [SerializeField]
    private InventoryUnit quickSlot3;

    [SerializeField]
    private InventoryUnit quickSlot4;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            if (quickSlot1 != null)
                quickSlot1.UseItem();

        if (Input.GetKeyDown(KeyCode.Alpha2))
            if (quickSlot2 != null)
                quickSlot2.UseItem();

        if (Input.GetKeyDown(KeyCode.Alpha3))
            if (quickSlot3 != null)
                quickSlot3.UseItem();

        if (Input.GetKeyDown(KeyCode.Alpha4))
            if (quickSlot4 != null)
                quickSlot4.UseItem();
    }
}
