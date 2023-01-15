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
        inven = Inventory.GetComponentsInChildren<InventoryUnit>();

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (Inventory.gameObject.activeSelf)
            {
                Inventory.gameObject.SetActive(false);
                InventoryOn = false;
            }
            else
            {
                Inventory.gameObject.SetActive(true);
                InventoryOn = true;
            }
        }

        if (Inventory.gameObject.activeSelf == true || GameManager.Instance.BuildMode == true)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else if (Inventory.gameObject.activeSelf == false || GameManager.Instance.BuildMode == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }


    }


    public void AddItem(InventoryItem inventoryItem)
    {

        for (int i = 0; i < inven.Length; i++)
        {
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
                PlayerManager.Instance.GainHp(inventoryItem.data.RecoverHp);
                PlayerManager.Instance.GainMp(inventoryItem.data.RecoverMp);
                break;

            case "Equipment":
                Debug.Log("검을 분해시켰다");
                break;

            default:
                break;

        }

    }

    public void EliminateItem(InventoryItem inventoryItem)
    {
        string Type = inventoryItem.data.Itemtype.ToString();

        switch (Type)
        {
            case "Potion":
                items.Remove(inventoryItem);
                Debug.Log(inventoryItem.data.name.ToString() + "이 사용되며, 파괴시킵니다. ");
                PlayerManager.Instance.GainHp(inventoryItem.data.RecoverHp);
                PlayerManager.Instance.GainMp(inventoryItem.data.RecoverMp);
                break;

            case "Equipment":
                items.Remove(inventoryItem);
                Debug.Log(inventoryItem.data.name.ToString() + "이 사용되며, 파괴시킵니다. ");
                break;

            default:
                break;

        }

        Inventory.UpdateUI();
    }

    private IEnumerator WaitUpdate()
    {
        yield return new WaitForSeconds (0.1f);
        Inventory.UpdateUI();

    }

}
