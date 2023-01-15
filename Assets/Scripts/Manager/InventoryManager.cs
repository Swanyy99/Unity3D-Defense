using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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

    [SerializeField]
    private InventoryUnit[] inven;

    public InventoryItem NowItem;

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

        inven = Inventory.GetComponentsInChildren<InventoryUnit>();

        //Inventory.UpdateUI();

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
        Debug.Log("호출됐음");
        
        for (int i = 0; i < inven.Length; i++)
        {
            Debug.Log("발동했음");

            if (inven[i].Item != null)
            {
                if (inven[i].Item.data.name == inventoryItem.data.name)
                {
                    inven[i].SetItemCount(items[i], 1);
                    Inventory.UpdateUI();
                    return;
                }
            }
        }

        for (int i = 0; i< inven.Length; i++)
        {
            if (inven[i].Item == null)
            {
                items.Add(inventoryItem);
                Inventory.UpdateUI();
                return;
            }
        }

        

        //Inventory.UpdateUI();
        ///*Inventory.*/UpdateUI();
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
                //items.Remove(inventoryItem);
                PlayerManager.Instance.GainHp(inventoryItem.data.RecoverHp);
                PlayerManager.Instance.GainMp(inventoryItem.data.RecoverMp);
                break;

            default:
                Debug.Log("검을 분해시켰다");
                //items.Remove(inventoryItem);
                break;

        }



        Inventory.UpdateUI();
    }

    public void EliminateItem(InventoryItem inventoryItem)
    {
        string Type = inventoryItem.data.Itemtype.ToString();

        switch (Type)
        {
            case "Potion":
                Debug.Log(inventoryItem.data.name.ToString() + "이 사용되며, 파괴시킵니다. ");
                items.Remove(inventoryItem);
                PlayerManager.Instance.GainHp(inventoryItem.data.RecoverHp);
                PlayerManager.Instance.GainMp(inventoryItem.data.RecoverMp);
                break;

            default:
                Debug.Log(inventoryItem.data.name.ToString() + "이 사용되며, 파괴시킵니다. ");
                Debug.Log("검을 분해시켰다");
                items.Remove(inventoryItem);
                break;

        }



        Inventory.UpdateUI();
    }

}
