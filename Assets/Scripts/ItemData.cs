using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Item/Item")]
public class ItemData : ScriptableObject
{
    public enum type
    {
        Equipment,
        Potion
        
    }
    public type Itemtype;
    public new string name;
    public int RecoverHp;
    public int RecoverMp;
    [TextArea(1, 4)]
    public string description;
    public GameObject UseEffect;
    public GameObject prefab;
    public Sprite icon;

}
