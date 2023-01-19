using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

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
        //icon.color = new Color(255, 255, 255, 255);
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

                if (tempHead != null)
                {
                    DragSlot.instance.dragSlot.AddItem2(tempHead);
                    DragSlot.instance.dragSlot.SetCount(tempHeadCount);
                }
                else
                {
                    DragSlot.instance.dragSlot.RemoveItem();
                }

            }

            // Armor
            if (this.gameObject.name == "ArmorSlot" && DragSlot.instance.dragSlot.Item.data.EquipType.ToString() == "Armor")
            {
                InventoryItem tempArmor = this.Item;
                int tempArmorCount = ItemCount;

                AddItem2(DragSlot.instance.dragSlot.Item);
                SetCount(DragSlot.instance.dragSlot.ItemCount);

                if (tempArmor != null)
                {
                    DragSlot.instance.dragSlot.AddItem2(tempArmor);
                    DragSlot.instance.dragSlot.SetCount(tempArmorCount);
                }
                else
                {
                    DragSlot.instance.dragSlot.RemoveItem();
                }

            }

            // Pants
            if (this.gameObject.name == "PantsSlot" && DragSlot.instance.dragSlot.Item.data.EquipType.ToString() == "Pants")
            {
                InventoryItem tempPants = this.Item;
                int tempPantsCount = ItemCount;

                AddItem2(DragSlot.instance.dragSlot.Item);
                SetCount(DragSlot.instance.dragSlot.ItemCount);

                if (tempPants != null)
                {
                    DragSlot.instance.dragSlot.AddItem2(tempPants);
                    DragSlot.instance.dragSlot.SetCount(tempPantsCount);
                }
                else
                {
                    DragSlot.instance.dragSlot.RemoveItem();
                }

            }

            // Glove
            if (this.gameObject.name == "GloveSlot" && DragSlot.instance.dragSlot.Item.data.EquipType.ToString() == "Glove")
            {
                InventoryItem tempGlove = this.Item;
                int tempGloveCount = ItemCount;

                AddItem2(DragSlot.instance.dragSlot.Item);
                SetCount(DragSlot.instance.dragSlot.ItemCount);

                if (tempGlove != null)
                {
                    DragSlot.instance.dragSlot.AddItem2(tempGlove);
                    DragSlot.instance.dragSlot.SetCount(tempGloveCount);
                }
                else
                {
                    DragSlot.instance.dragSlot.RemoveItem();
                }

            }

            // Shoes
            if (this.gameObject.name == "ShoesSlot" && DragSlot.instance.dragSlot.Item.data.EquipType.ToString() == "Shoes")
            {
                InventoryItem tempShoes = this.Item;
                int tempShoesCount = ItemCount;

                AddItem2(DragSlot.instance.dragSlot.Item);
                SetCount(DragSlot.instance.dragSlot.ItemCount);

                if (tempShoes != null)
                {
                    DragSlot.instance.dragSlot.AddItem2(tempShoes);
                    DragSlot.instance.dragSlot.SetCount(tempShoesCount);
                }
                else
                {
                    DragSlot.instance.dragSlot.RemoveItem();
                }

            }

            // Weapon
            if (this.gameObject.name == "WeaponSlot" && DragSlot.instance.dragSlot.Item.data.EquipType.ToString() == "Weapon")
            {
                InventoryItem tempWeapon = this.Item;
                int tempWeaponCount = ItemCount;

                AddItem2(DragSlot.instance.dragSlot.Item);
                SetCount(DragSlot.instance.dragSlot.ItemCount);

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
}
