using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Item/Item")]
public class ItemData : ScriptableObject
{
    public enum type
    {
        Equipment,
        Potion,
        Material
    }

    public enum equipType
    {
        None,
        Head,
        Armor,
        Glove,
        Pants,
        Shoes,
        Weapon
    }

    [Header("Type")]
    public type Itemtype;
    public equipType EquipType;

    [Header("Prefab")]
    public GameObject prefab;
    public Sprite icon;
    public GameObject UseEffect;

    [Header("Basic")]
    public new string name;
    [TextArea(1, 4)]
    public string description;
    public int PurchaseCost;
    public int SellCost;

    [Header("Potion")]
    public int RecoverHp;
    public int RecoverMp;

    [Header("Stat")]
    public int STR_plus;
    public int DEF_plus;
    public int DEX_plus;
    public int INT_plus;
    public int MaxHP_plus;
    public int MaxMP_plus;
    public int HpRecover_plus;
    public int MpRecover_plus;

}
