using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUnit : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler /*, IPointerEnterHandler, IPointerExitHandler*/
{
    public InventoryItem Item;

    [SerializeField]
    private Button useButton;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Image dragIcon;

    [SerializeField]
    private TextMeshProUGUI count;

    public int ItemCount = 0;

    private InventoryUnit[] QuickInven;

    [SerializeField]
    private InventoryUnit quickSlot1;

    [SerializeField]
    private InventoryUnit quickSlot2;

    [SerializeField]
    private InventoryUnit quickSlot3;

    [SerializeField]
    private InventoryUnit quickSlot4;

    [SerializeField]
    private InventoryUnit HeadSlot;
    [SerializeField]
    private InventoryUnit ArmorSlot;
    [SerializeField]
    private InventoryUnit PantsSlot;
    [SerializeField]
    private InventoryUnit GloveSlot;
    [SerializeField]
    private InventoryUnit ShoesSlot;
    [SerializeField]
    private InventoryUnit WeaponSlot;

    [SerializeField]
    private InventoryUI QuickInventory;

    [SerializeField]
    private GameObject ItemTooltipUI;

    [SerializeField]
    private ItemTooltipUI tooltip;

    [SerializeField]
    private TextMeshProUGUI ItemName;
    [SerializeField]
    private TextMeshProUGUI ItemType;
    [SerializeField]
    private TextMeshProUGUI ItemDescriprion;
    [SerializeField]
    private TextMeshProUGUI ItemCost;

    [SerializeField]
    private EquipSlotSaveStat Head;
    [SerializeField]
    private EquipSlotSaveStat Armor;
    [SerializeField]
    private EquipSlotSaveStat Pants;
    [SerializeField] 
    private EquipSlotSaveStat Weapon;
    [SerializeField]
    private EquipSlotSaveStat Glove;
    [SerializeField]
    private EquipSlotSaveStat Shoes;

    [SerializeField]
    private Image ShopSellPosImage;

    private string SlotName;

    InventoryItem EquiptempItem;
    int EquiptempItemCount;

    private void Start()
    {
        
    }

    public void AddItem(InventoryItem inventoryItem)
    {
        useButton.interactable = true;
        icon.sprite = inventoryItem.data.icon;
        icon.color = new Color(255, 255, 255, 255);
        this.Item = inventoryItem;
        QuickSlotAdd();
    }



    public void AddItem2(InventoryItem inventoryItem)
    {
        useButton.interactable = true;
        icon.sprite = inventoryItem.data.icon;
        icon.color = new Color(255, 255, 255, 255);
        this.Item = inventoryItem;
    }

    public void SetItemCount(int num)
    {
        ItemCount += num;
        count.text = ItemCount.ToString();
        if (count.text == "1")
            count.text = "";
    }

    public void SetCount(int num)
    {
        ItemCount = num;
        count.text = ItemCount.ToString();
        if (count.text == "1")
            count.text = "";
    }

    public void RemoveItem()
    {
        icon.sprite = null;
        icon.color = new Color(255, 255, 255, 0);
        count.text = "";
        this.Item = null;
        ItemCount = 0;
    }

    public void UseItem()
    {
        if (this.Item != null)
        {
            if (this.Item.data.Itemtype.ToString() != "Equipment")
            {
                if (ItemCount > 1)
                {
                    Item.Use();
                    SetItemCount(-1);
                    Debug.Log("������ ������ 2���̻��϶� ����߽��ϴ�.");
                }

                else if (ItemCount == 1)
                {
                    Item.Use();
                    icon.sprite = null;
                    icon.color = new Color(255, 255, 255, 0);
                    count.text = "";
                    this.Item = null;
                    Debug.Log("������ ������ 1���϶� ����߽��ϴ�.");
                }
                return;
            }

            else if (this.Item.data.Itemtype.ToString() == "Equipment")
            {
                InventoryManager.Instance.EquipUIShow();
                StartCoroutine(delayEquip());
            }

        }
    }


    public string GetItemName()
    {
        return this.Item.data.name;
    }

    public void ShowTooltip()
    {

        if (this.Item != null)
        {
            ItemName.text = this.Item.data.name;
            ItemDescriprion.text = this.Item.data.description;
            ItemCost.text = "���Ű��� : " + this.Item.data.PurchaseCost + " G\n" +
                            "�ǸŰ��� : " + this.Item.data.SellCost + " G";
            ItemType.text = null;
            if (this.Item.data.Itemtype.ToString() == "Potion")
            {
                ItemType.text = "[�Ҹ�ǰ]";
            }

            else if ((this.Item.data.Itemtype.ToString() == "Material"))
            {
                ItemType.text = "[����]";
            }

            else if ((this.Item.data.Itemtype.ToString() == "Equipment"))
            {
                if (this.Item.data.EquipType.ToString() == "Head")
                    ItemType.text = "[����]";
                if (this.Item.data.EquipType.ToString() == "Armor")
                    ItemType.text = "[����]";
                if (this.Item.data.EquipType.ToString() == "Pants")
                    ItemType.text = "[����]";
                if (this.Item.data.EquipType.ToString() == "Glove")
                    ItemType.text = "[�尩]";
                if (this.Item.data.EquipType.ToString() == "Shoes")
                    ItemType.text = "[�Ź�]";
                if (this.Item.data.EquipType.ToString() == "Weapon")
                    ItemType.text = "[����]";
            }
            else
                ItemType.text = "[???]";

            ItemTooltipUI.SetActive(true);
        }

    }

    public void HideToolTip()
    {
        if (ItemTooltipUI.activeSelf)
            ItemTooltipUI.SetActive(false);
    }

    public void IsThereITEM()
    {
        if (this.Item == null)
            HideToolTip();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) 
        {
            if (this.Item != null)
                UseItem();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (this.Item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(this.icon);
            DragSlot.instance.transform.position = eventData.position;
            ShopSellPosImage.raycastTarget = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (this.Item != null)
            DragSlot.instance.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
        ItemTooltipUI.gameObject.SetActive(false);
        ShopSellPosImage.raycastTarget = false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
            ChangeSlot();
    }


    private void ChangeSlot()
    {
        if (!this.gameObject.tag.Equals("EquipUI"))
        {

            if (!DragSlot.instance.dragSlot.gameObject.tag.Equals("EquipUI"))
            {

                InventoryItem tempItem = this.Item;
                int tempItemCount = ItemCount;

                AddItem2(DragSlot.instance.dragSlot.Item);
                SetCount(DragSlot.instance.dragSlot.ItemCount);

                if (tempItem != null)
                {
                    DragSlot.instance.dragSlot.AddItem2(tempItem);
                    DragSlot.instance.dragSlot.SetCount(tempItemCount);
                }
                else
                {
                    DragSlot.instance.dragSlot.RemoveItem();
                }

                Head.UnEquipDetect();
                Armor.UnEquipDetect();
                Glove.UnEquipDetect();
                Shoes.UnEquipDetect();
                Pants.UnEquipDetect();
                Weapon.UnEquipDetect();
                return;
            }

            else if (DragSlot.instance.dragSlot.gameObject.tag.Equals("EquipUI"))
            {
                
                if (this.Item != null && DragSlot.instance.dragSlot.Item.data.EquipType.ToString() ==
                    this.Item.data.EquipType.ToString())
                {
                    InventoryItem tempItem = this.Item;
                    int tempItemCount = ItemCount;

                    string type;
                    type = this.Item.data.EquipType.ToString();
                    switch (type)
                    {
                        case "Head": Head.SaveStat(ThisSTR(), ThisDEF(), ThisDEX(), ThisINT(), ThisMHP(), ThisMMP(), ThisHPR(), ThisMPR()); break;
                        case "Armor": Armor.SaveStat(ThisSTR(), ThisDEF(), ThisDEX(), ThisINT(), ThisMHP(), ThisMMP(), ThisHPR(), ThisMPR()); break;
                        case "Pants": Pants.SaveStat(ThisSTR(), ThisDEF(), ThisDEX(), ThisINT(), ThisMHP(), ThisMMP(), ThisHPR(), ThisMPR()); break;
                        case "Glove": Glove.SaveStat(ThisSTR(), ThisDEF(), ThisDEX(), ThisINT(), ThisMHP(), ThisMMP(), ThisHPR(), ThisMPR()); break;
                        case "Shoes": Shoes.SaveStat(ThisSTR(), ThisDEF(), ThisDEX(), ThisINT(), ThisMHP(), ThisMMP(), ThisHPR(), ThisMPR()); break;
                        case "Weapon": Weapon.SaveStat(ThisSTR(), ThisDEF(), ThisDEX(), ThisINT(), ThisMHP(), ThisMMP(), ThisHPR(), ThisMPR()); break;
                        default: break;
                    }

                    AddItem2(DragSlot.instance.dragSlot.Item);
                    SetCount(DragSlot.instance.dragSlot.ItemCount);

                    if (tempItem != null)
                    {
                        DragSlot.instance.dragSlot.AddItem2(tempItem);
                        DragSlot.instance.dragSlot.SetCount(tempItemCount);
                    }
                    else
                    {
                        DragSlot.instance.dragSlot.RemoveItem();
                    }

                    return;
                }

                else if (this.Item == null)
                {
                    InventoryItem tempItem = this.Item;
                    int tempItemCount = ItemCount;

                    AddItem2(DragSlot.instance.dragSlot.Item);
                    SetCount(DragSlot.instance.dragSlot.ItemCount);

                    if (tempItem != null)
                    {
                        DragSlot.instance.dragSlot.AddItem2(tempItem);
                        DragSlot.instance.dragSlot.SetCount(tempItemCount);
                    }
                    else
                    {
                        DragSlot.instance.dragSlot.RemoveItem();
                    }

                    Head.UnEquipDetect();
                    Armor.UnEquipDetect();
                    Glove.UnEquipDetect();
                    Shoes.UnEquipDetect();
                    Pants.UnEquipDetect();
                    Weapon.UnEquipDetect();
                    return;
                }
                return;
            }
            return;
        }


        if (this.gameObject.tag.Equals("EquipUI"))
        {
            string tempName = this.gameObject.name;

            switch (tempName)
            {
                case "HeadSlot": SlotName = "Head"; break;
                case "ArmorSlot": SlotName = "Armor"; break;
                case "PantsSlot": SlotName = "Pants"; break;
                case "GloveSlot": SlotName = "Glove"; break;
                case "ShoesSlot": SlotName = "Shoes"; break;
                case "WeaponSlot": SlotName = "Weapon"; break;

            }

            if (SlotName == DragSlot.instance.dragSlot.Item.data.EquipType.ToString())
            {
                InventoryItem tempItem = this.Item;
                int tempCount = ItemCount;

                AddItem2(DragSlot.instance.dragSlot.Item);
                SetCount(DragSlot.instance.dragSlot.ItemCount);

                string Type = this.gameObject.name;

                switch (Type)
                {
                    case "HeadSlot": Head.SaveStat(STR(), DEF(), DEX(), INT(), MHP(), MMP(), HPR(), MPR()); break;
                    case "ArmorSlot": Armor.SaveStat(STR(), DEF(), DEX(), INT(), MHP(), MMP(), HPR(), MPR()); break;
                    case "PantsSlot": Pants.SaveStat(STR(), DEF(), DEX(), INT(), MHP(), MMP(), HPR(), MPR()); break;
                    case "GloveSlot": Glove.SaveStat(STR(), DEF(), DEX(), INT(), MHP(), MMP(), HPR(), MPR()); break;
                    case "ShoesSlot": Shoes.SaveStat(STR(), DEF(), DEX(), INT(), MHP(), MMP(), HPR(), MPR()); break;
                    case "WeaponSlot": Weapon.SaveStat(STR(), DEF(), DEX(), INT(), MHP(), MMP(), HPR(), MPR()); break;
                }

                if (tempItem != null)
                {
                    DragSlot.instance.dragSlot.AddItem2(tempItem);
                    DragSlot.instance.dragSlot.SetCount(tempCount);
                }
                else
                {
                    DragSlot.instance.dragSlot.RemoveItem();
                }
                return;
            }
        }
        return;
    }

    public void EquipItem()
    {
        ItemTooltipUI.gameObject.SetActive(false);

        Head = GameObject.Find("HeadSlot").GetComponent<EquipSlotSaveStat>();
        Armor = GameObject.Find("ArmorSlot").GetComponent<EquipSlotSaveStat>();
        Pants = GameObject.Find("PantsSlot").GetComponent<EquipSlotSaveStat>();
        Weapon = GameObject.Find("WeaponSlot").GetComponent<EquipSlotSaveStat>();
        Glove = GameObject.Find("GloveSlot").GetComponent<EquipSlotSaveStat>();
        Shoes = GameObject.Find("ShoesSlot").GetComponent<EquipSlotSaveStat>();

        HeadSlot = GameObject.Find("HeadSlot").GetComponent<InventoryUnit>();
        ArmorSlot = GameObject.Find("ArmorSlot").GetComponent<InventoryUnit>();
        PantsSlot = GameObject.Find("PantsSlot").GetComponent<InventoryUnit>();
        GloveSlot = GameObject.Find("GloveSlot").GetComponent<InventoryUnit>();
        ShoesSlot = GameObject.Find("ShoesSlot").GetComponent<InventoryUnit>();
        WeaponSlot = GameObject.Find("WeaponSlot").GetComponent<InventoryUnit>();

        string typeName = Item.data.EquipType.ToString();
        
        switch (typeName)
        {
            case "Head":
                EquiptempItem = HeadSlot.Item;
                EquiptempItemCount = HeadSlot.ItemCount;

                HeadSlot.AddItem2(Item);
                HeadSlot.SetCount(ItemCount);

                Head.SaveStat(ThisSTR(), ThisDEF(), ThisDEX(), ThisINT(), ThisMHP(), ThisMMP(), ThisHPR(), ThisMPR()); break;

            case "Armor":
                EquiptempItem = ArmorSlot.Item;
                EquiptempItemCount = ArmorSlot.ItemCount;

                ArmorSlot.AddItem2(Item);
                ArmorSlot.SetCount(ItemCount);

                Armor.SaveStat(ThisSTR(), ThisDEF(), ThisDEX(), ThisINT(), ThisMHP(), ThisMMP(), ThisHPR(), ThisMPR()); break;

            case "Pants":
                EquiptempItem = PantsSlot.Item;
                EquiptempItemCount = PantsSlot.ItemCount;

                PantsSlot.AddItem2(Item);
                PantsSlot.SetCount(ItemCount);

                Pants.SaveStat(ThisSTR(), ThisDEF(), ThisDEX(), ThisINT(), ThisMHP(), ThisMMP(), ThisHPR(), ThisMPR()); break;

            case "Glove":
                EquiptempItem = GloveSlot.Item;
                EquiptempItemCount = GloveSlot.ItemCount;

                GloveSlot.AddItem2(Item);
                GloveSlot.SetCount(ItemCount);

                Glove.SaveStat(ThisSTR(), ThisDEF(), ThisDEX(), ThisINT(), ThisMHP(), ThisMMP(), ThisHPR(), ThisMPR()); break;

            case "Shoes":
                EquiptempItem = ShoesSlot.Item;
                EquiptempItemCount = ShoesSlot.ItemCount;

                ShoesSlot.AddItem2(Item);
                ShoesSlot.SetCount(ItemCount);

                Shoes.SaveStat(ThisSTR(), ThisDEF(), ThisDEX(), ThisINT(), ThisMHP(), ThisMMP(), ThisHPR(), ThisMPR()); break;

            case "Weapon":
                EquiptempItem = WeaponSlot.Item;
                EquiptempItemCount = WeaponSlot.ItemCount;

                WeaponSlot.AddItem2(Item);
                WeaponSlot.SetCount(ItemCount);

                Weapon.SaveStat(ThisSTR(), ThisDEF(), ThisDEX(), ThisINT(), ThisMHP(), ThisMMP(), ThisHPR(), ThisMPR()); break;

            default: break;
        }

        if (EquiptempItem != null)
        {
            AddItem2(EquiptempItem);
            SetCount(EquiptempItemCount);
        }
        else
        {
            RemoveItem();
        }

        return;
    }

    private void QuickSlotAdd()
    {
        QuickInven = QuickInventory.GetComponentsInChildren<InventoryUnit>();

        for (int i = 0; i < QuickInven.Length; i++)
        {
            if (QuickInven[i].Item != null)
            {
                if (this.Item.data.Itemtype.ToString() != "Equipment")
                {
                    if (QuickInven[i].Item.data.name == this.Item.data.name)
                    {
                        QuickInven[i].SetItemCount(1);
                        RemoveItem();
                        return;
                    }
                }
            }
        }

    }

    private int STR()
    {
        return DragSlot.instance.dragSlot.Item.data.STR_plus;
    }
    private int ThisSTR()
    {
        return this.Item.data.STR_plus;
    }
    private int DEF()
    {
        return DragSlot.instance.dragSlot.Item.data.DEF_plus;
    }
    private int ThisDEF()
    {
        return this.Item.data.DEF_plus;
    }
    private int DEX()
    {
        return DragSlot.instance.dragSlot.Item.data.DEX_plus;
    }
    private int ThisDEX()
    {
        return this.Item.data.DEX_plus;
    }
    private int INT()
    {
        return DragSlot.instance.dragSlot.Item.data.INT_plus;
    }
    private int ThisINT()
    {
        return this.Item.data.INT_plus;
    }
    private int MHP()
    {
        return DragSlot.instance.dragSlot.Item.data.MaxHP_plus;
    }
    private int ThisMHP()
    {
        return this.Item.data.MaxHP_plus;
    }
    private int MMP()
    {
        return DragSlot.instance.dragSlot.Item.data.MaxMP_plus;
    }
    private int ThisMMP()
    {
        return this.Item.data.MaxMP_plus;
    }
    private int HPR()
    {
        return DragSlot.instance.dragSlot.Item.data.HpRecover_plus;
    }
    private int ThisHPR()
    {
        return this.Item.data.HpRecover_plus;
    }
    private int MPR()
    {
        return DragSlot.instance.dragSlot.Item.data.MpRecover_plus;
    }
    private int ThisMPR()
    {
        return this.Item.data.MpRecover_plus;
    }
    IEnumerator delayEquip()
    {
        yield return new WaitForSeconds(0.05f);
        EquipItem();
    }

}
