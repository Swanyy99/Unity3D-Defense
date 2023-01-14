using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingleTon<InventoryManager>
{
    [SerializeField]
    private InventoryUI Inventory;
    [SerializeField]
    private GameObject TooltipUI;
    public List<InventoryItem> items = new List<InventoryItem>();

    private GameObject player;

    public bool InventoryOn;

    //public ItemData.type Type;
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
                InventoryOn = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Inventory.gameObject.SetActive(true);
                InventoryOn = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
        }
    }

    private void FixedUpdate()
    {
        Inventory.UpdateUI();

        if (Inventory.gameObject.activeSelf)
            Cursor.lockState = CursorLockMode.Confined;
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            TooltipUI.SetActive(false);
        }
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

    public void UseItem(InventoryItem inventoryItem)
    {
        string Type = inventoryItem.data.Itemtype.ToString();

        switch (Type)
        {
            case "Potion":
                Debug.Log("포션이 발동한다");
                items.Remove(inventoryItem);
                PlayerManager.Instance.GainHp(inventoryItem.data.RecoverHp);
                PlayerManager.Instance.GainMp(inventoryItem.data.RecoverMp);
                break;

            default: 
                break;

        }

        //if (inventoryItem.data.Itemtype.ToString() == "Potion")
        //{
        //    Debug.Log("포션이 발동한다");
        //    items.Remove(inventoryItem);
        //    PlayerManager.Instance.GainHp(inventoryItem.data.RecoverHp);
        //}

        Inventory.UpdateUI();
    }

}
