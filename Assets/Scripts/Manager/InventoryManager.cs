using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingleTon<InventoryManager>
{
    [SerializeField]
    private InventoryUI Inventory;
    public List<InventoryItem> items = new List<InventoryItem>();

    private GameObject player;

    //Vector3 PlayerPos;
    //Vector3 PlayerRot;

    private void Start()
    {
        Inventory.UpdateUI();
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        //PlayerPos = new Vector3 (player.transform.position.x , player.transform.position.y, player.transform.position.z + 2);
        //PlayerRot = new Vector3 (player.transform.rotation.x , player.transform.rotation.y, player.transform.rotation.z + 2);
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (Inventory.gameObject.activeSelf)
            {
                Inventory.gameObject.SetActive(false);
            }
            else
            {
                Inventory.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    private void FixedUpdate()
    {
        Inventory.UpdateUI();

    }

    public void AddItem(InventoryItem inventoryItem)
    {
        items.Add(inventoryItem);
        Inventory.UpdateUI();
    }

    public void DropItem(InventoryItem inventoryItem)
    {
        items.Remove(inventoryItem);
        Inventory.UpdateUI();

        Instantiate(inventoryItem.data.prefab, player.transform.position, player.transform.rotation);
    }
}
