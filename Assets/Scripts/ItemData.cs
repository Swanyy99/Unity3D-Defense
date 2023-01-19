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

    public type Itemtype;
    public new string name;
    public int RecoverHp;
    public int RecoverMp;
    public int SellCost;
    [TextArea(1, 4)]
    public string description;
    public GameObject UseEffect;
    public GameObject prefab;
    public Sprite icon;

}
