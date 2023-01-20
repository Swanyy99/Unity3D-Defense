using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EquipSlotSaveStat : MonoBehaviour
{

    private InventoryUnit slot;

    [SerializeField]
    private TextMeshProUGUI EquipAllStatUI;

    [SerializeField]
    private EquipmentUI EquipUI;

    public int STR_plus { get; private set; }
    public int DEF_plus { get; private set; }
    public int DEX_plus { get; private set; }
    public int INT_plus { get; private set; }
    public int MaxHP_plus { get; private set; }
    public int MaxMP_plus { get; private set; }
    public int HpRecover_plus { get; private set; }
    public int MpRecover_plus { get; private set; }


    private void Start()
    {
        slot = GetComponent<InventoryUnit>();
    }

    public void SaveStat(int Str, int Def, int Dex, int Int, int MaxHp, int MaxMp, int HpRecover, int MpRecover)
    {
        //EquipUI = GetComponentInParent<EquipmentUI>();

        STR_plus = Str;
        DEF_plus = Def;
        DEX_plus = Dex;
        INT_plus = Int;
        MaxHP_plus = MaxHp;
        MaxMP_plus = MaxMp;
        HpRecover_plus = HpRecover;
        MpRecover_plus = MpRecover;
        EquipUI.UpdateAllstat();
        UnEquipDetect();
    }

    private void UiUpdate()
    {
        EquipAllStatUI.text =
            "STR\t+" + STR_plus + "\n" +
            "DEF\t+" + DEF_plus + "\n" +
            "DEX\t+" + DEX_plus + "\n" +
            "INT\t+" + INT_plus + "\n" +
            "MHP\t+" + MaxHP_plus + "\n" +
            "MMP\t+" + MaxMP_plus + "\n" +
            "HPR\t+" + HpRecover_plus + "\n" +
            "MPR\t+" + MpRecover_plus;
    }

    public void UnEquipDetect()
    {
        //EquipUI = GetComponentInParent<EquipmentUI>();
        if (slot == null)
            return;

        if (slot.Item == null)
        {
            STR_plus = 0;
            DEF_plus = 0;
            DEX_plus = 0;
            INT_plus = 0;
            MaxHP_plus = 0;
            MaxMP_plus = 0;
            HpRecover_plus = 0;
            MpRecover_plus = 0;
            EquipUI.UpdateAllstat();
            return;
        }

        
        

    }

}
