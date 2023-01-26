using System.Collections;
using System.Collections.Generic;
using TMPro;
//using Unity.VisualScripting;
//using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using static UnityEditor.Progress;

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
                    Debug.Log("아이템 갯수가 2개이상일때 사용했습니다.");
                }

                else if (ItemCount == 1)
                {
                    Item.UseEliminate();
                    icon.sprite = null;
                    icon.color = new Color(255, 255, 255, 0);
                    count.text = "";
                    this.Item = null;
                    Debug.Log("아이템 갯수가 1개일때 사용했습니다.");
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
            ItemCost.text = "구매가격 : " + this.Item.data.PurchaseCost + " G\n" +
                            "판매가격 : " + this.Item.data.SellCost + " G";
            ItemType.text = null;
            if (this.Item.data.Itemtype.ToString() == "Potion")
            {
                ItemType.text = "[소모품]";
            }

            else if ((this.Item.data.Itemtype.ToString() == "Material"))
            {
                ItemType.text = "[잡템]";
            }

            else if ((this.Item.data.Itemtype.ToString() == "Equipment"))
            {
                if (this.Item.data.EquipType.ToString() == "Head")
                    ItemType.text = "[모자]";
                if (this.Item.data.EquipType.ToString() == "Armor")
                    ItemType.text = "[상의]";
                if (this.Item.data.EquipType.ToString() == "Pants")
                    ItemType.text = "[하의]";
                if (this.Item.data.EquipType.ToString() == "Glove")
                    ItemType.text = "[장갑]";
                if (this.Item.data.EquipType.ToString() == "Shoes")
                    ItemType.text = "[신발]";
                if (this.Item.data.EquipType.ToString() == "Weapon")
                    ItemType.text = "[무기]";
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
            //gameObject.transform.SetAsLastSibling();
            //icon.color = new Color(255, 255, 255, 0);
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

            //if (!this.gameObject.tag.Equals("ShopUI"))
            //else
            //    SellItem();
    }

    //private void SellItem()
    //{
    //    BuildManager.Instance.GoldChange(DragSlot.instance.dragSlot.Item.data.SellCost);
    //    DragSlot.instance.dragSlot.RemoveItem();
    //}

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
            // Head
            if (this.gameObject.name == "HeadSlot" && DragSlot.instance.dragSlot.Item.data.EquipType.ToString() == "Head")
            {

                InventoryItem tempHead = this.Item;
                int tempHeadCount = ItemCount;

                AddItem2(DragSlot.instance.dragSlot.Item);
                SetCount(DragSlot.instance.dragSlot.ItemCount);

                Head.SaveStat(STR(), DEF(), DEX(), INT(), MHP(), MMP(), HPR(), MPR());

                if (tempHead != null)
                {
                    DragSlot.instance.dragSlot.AddItem2(tempHead);
                    DragSlot.instance.dragSlot.SetCount(tempHeadCount);
                }
                else
                {
                    DragSlot.instance.dragSlot.RemoveItem();
                }
                return;

            }

            // Armor
            if (this.gameObject.name == "ArmorSlot" && DragSlot.instance.dragSlot.Item.data.EquipType.ToString() == "Armor")
            {

                InventoryItem tempArmor = this.Item;
                int tempArmorCount = ItemCount;

                AddItem2(DragSlot.instance.dragSlot.Item);
                SetCount(DragSlot.instance.dragSlot.ItemCount);

                Armor.SaveStat(STR(), DEF(), DEX(), INT(), MHP(), MMP(), HPR(), MPR());

                if (tempArmor != null)
                {
                    DragSlot.instance.dragSlot.AddItem2(tempArmor);
                    DragSlot.instance.dragSlot.SetCount(tempArmorCount);
                }
                else
                {
                    DragSlot.instance.dragSlot.RemoveItem();
                }
                return;

            }

            // Pants
            if (this.gameObject.name == "PantsSlot" && DragSlot.instance.dragSlot.Item.data.EquipType.ToString() == "Pants")
            {

                InventoryItem tempPants = this.Item;
                int tempPantsCount = ItemCount;

                AddItem2(DragSlot.instance.dragSlot.Item);
                SetCount(DragSlot.instance.dragSlot.ItemCount);

                Pants.SaveStat(STR(), DEF(), DEX(), INT(), MHP(), MMP(), HPR(), MPR());

                if (tempPants != null)
                {
                    DragSlot.instance.dragSlot.AddItem2(tempPants);
                    DragSlot.instance.dragSlot.SetCount(tempPantsCount);
                }
                else
                {
                    DragSlot.instance.dragSlot.RemoveItem();
                }
                return;
            }

            // Glove
            if (this.gameObject.name == "GloveSlot" && DragSlot.instance.dragSlot.Item.data.EquipType.ToString() == "Glove")
            {

                InventoryItem tempGlove = this.Item;
                int tempGloveCount = ItemCount;

                AddItem2(DragSlot.instance.dragSlot.Item);
                SetCount(DragSlot.instance.dragSlot.ItemCount);

                Glove.SaveStat(STR(), DEF(), DEX(), INT(), MHP(), MMP(), HPR(), MPR());

                if (tempGlove != null)
                {
                    DragSlot.instance.dragSlot.AddItem2(tempGlove);
                    DragSlot.instance.dragSlot.SetCount(tempGloveCount);
                }
                else
                {
                    DragSlot.instance.dragSlot.RemoveItem();
                }
                return;
            }

            // Shoes
            if (this.gameObject.name == "ShoesSlot" && DragSlot.instance.dragSlot.Item.data.EquipType.ToString() == "Shoes")
            {

                InventoryItem tempShoes = this.Item;
                int tempShoesCount = ItemCount;

                AddItem2(DragSlot.instance.dragSlot.Item);
                SetCount(DragSlot.instance.dragSlot.ItemCount);

                Shoes.SaveStat(STR(), DEF(), DEX(), INT(), MHP(), MMP(), HPR(), MPR());

                if (tempShoes != null)
                {
                    DragSlot.instance.dragSlot.AddItem2(tempShoes);
                    DragSlot.instance.dragSlot.SetCount(tempShoesCount);
                }
                else
                {
                    DragSlot.instance.dragSlot.RemoveItem();
                }
                return;
            }

            // Weapon
            if (this.gameObject.name == "WeaponSlot" && DragSlot.instance.dragSlot.Item.data.EquipType.ToString() == "Weapon")
            {

                InventoryItem tempWeapon = this.Item;
                int tempWeaponCount = ItemCount;

                AddItem2(DragSlot.instance.dragSlot.Item);
                SetCount(DragSlot.instance.dragSlot.ItemCount);

                Weapon.SaveStat(STR(), DEF(), DEX(), INT(), MHP(), MMP(), HPR(), MPR());

                if (tempWeapon != null)
                {
                    DragSlot.instance.dragSlot.AddItem2(tempWeapon);
                    DragSlot.instance.dragSlot.SetCount(tempWeaponCount);
                }
                else
                {
                    DragSlot.instance.dragSlot.RemoveItem();
                }

            }

            return;

        }

        return;
        
    }

    public void EquipItem()
    {


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

        Debug.Log("EquipItem은 발동했음");
        ItemTooltipUI.gameObject.SetActive(false);


        if (Item.data.EquipType.ToString().Equals("Head")) 
        {
            Debug.Log("투구인거 감지함!");

            InventoryItem tempItem = HeadSlot.Item;
            int tempItemCount = HeadSlot.ItemCount;

            HeadSlot.AddItem2(Item);
            HeadSlot.SetCount(ItemCount);

            Head.SaveStat(ThisSTR(), ThisDEF(), ThisDEX(), ThisINT(), ThisMHP(), ThisMMP(), ThisHPR(), ThisMPR());

            if (tempItem != null)
            {
                AddItem2(tempItem);
                SetCount(tempItemCount);
            }
            else
            {
                RemoveItem();
            }

            return;
        }

        if (Item.data.EquipType.ToString().Equals("Armor"))
        {
            Debug.Log("갑빠인거 감지함!");

            InventoryItem tempItem = ArmorSlot.Item;
            int tempItemCount = ArmorSlot.ItemCount;

            ArmorSlot.AddItem2(Item);
            ArmorSlot.SetCount(ItemCount);

            Armor.SaveStat(ThisSTR(), ThisDEF(), ThisDEX(), ThisINT(), ThisMHP(), ThisMMP(), ThisHPR(), ThisMPR());

            if (tempItem != null)
            {
                AddItem2(tempItem);
                SetCount(tempItemCount);
            }
            else
            {
                RemoveItem();
            }

            return;
        }

        if (Item.data.EquipType.ToString().Equals("Pants"))
        {
            Debug.Log("바지인거 감지함!");

            InventoryItem tempItem = PantsSlot.Item;
            int tempItemCount = PantsSlot.ItemCount;

            PantsSlot.AddItem2(Item);
            PantsSlot.SetCount(ItemCount);

            Pants.SaveStat(ThisSTR(), ThisDEF(), ThisDEX(), ThisINT(), ThisMHP(), ThisMMP(), ThisHPR(), ThisMPR());

            if (tempItem != null)
            {
                AddItem2(tempItem);
                SetCount(tempItemCount);
            }
            else
            {
                RemoveItem();
            }

            return;
        }

        if (Item.data.EquipType.ToString().Equals("Glove"))
        {
            Debug.Log("장갑인거 감지함!");

            InventoryItem tempItem = GloveSlot.Item;
            int tempItemCount = GloveSlot.ItemCount;

            GloveSlot.AddItem2(Item);
            GloveSlot.SetCount(ItemCount);

            Glove.SaveStat(ThisSTR(), ThisDEF(), ThisDEX(), ThisINT(), ThisMHP(), ThisMMP(), ThisHPR(), ThisMPR());

            if (tempItem != null)
            {
                AddItem2(tempItem);
                SetCount(tempItemCount);
            }
            else
            {
                RemoveItem();
            }

            return;
        }

        if (Item.data.EquipType.ToString().Equals("Shoes"))
        {
            Debug.Log("신발인거 감지함!");

            InventoryItem tempItem = ShoesSlot.Item;
            int tempItemCount = ShoesSlot.ItemCount;

            ShoesSlot.AddItem2(Item);
            ShoesSlot.SetCount(ItemCount);

            Shoes.SaveStat(ThisSTR(), ThisDEF(), ThisDEX(), ThisINT(), ThisMHP(), ThisMMP(), ThisHPR(), ThisMPR());

            if (tempItem != null)
            {
                AddItem2(tempItem);
                SetCount(tempItemCount);
            }
            else
            {
                RemoveItem();
            }

            return;
        }

        if (Item.data.EquipType.ToString().Equals("Weapon"))
        {
            Debug.Log("무기인거 감지함!");

            InventoryItem tempItem = WeaponSlot.Item;
            int tempItemCount = WeaponSlot.ItemCount;

            WeaponSlot.AddItem2(Item);
            WeaponSlot.SetCount(ItemCount);

            Weapon.SaveStat(ThisSTR(), ThisDEF(), ThisDEX(), ThisINT(), ThisMHP(), ThisMMP(), ThisHPR(), ThisMPR());

            if (tempItem != null)
            {
                AddItem2(tempItem);
                SetCount(tempItemCount);
            }
            else
            {
                RemoveItem();
            }

            return;
        }

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
