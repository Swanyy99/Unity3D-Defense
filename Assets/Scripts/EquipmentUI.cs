using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentUI : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    Vector3 FirstMousePos;
    Vector3 GAP;

    [SerializeField]
    private TextMeshProUGUI AllStatUI;

    private EquipSlotSaveStat[] EquipSlot;

    public int STR_plus { get; private set; }
    public int DEF_plus { get; private set; }
    public int DEX_plus { get; private set; }
    public int INT_plus { get; private set; }
    public int MaxHP_plus { get; private set; }
    public int MaxMP_plus { get; private set; }
    public int HpRecover_plus { get; private set; }
    public int MpRecover_plus { get; private set; }


    private void OnEnable()
    {
        gameObject.transform.SetAsLastSibling();

    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
        UIManager.Instance.UI_On();
        UIManager.Instance.SetMouse();
    }

    public void UpdateAllstat()
    {
        EquipSlot = gameObject.GetComponentsInChildren<EquipSlotSaveStat>();
        STR_plus = 0;
        DEF_plus = 0;
        DEX_plus = 0;
        INT_plus = 0;
        MaxHP_plus = 0;
        MaxMP_plus = 0;
        HpRecover_plus = 0;
        MpRecover_plus = 0;

        for (int i = 0; i < EquipSlot.Length; i++)
        {
            STR_plus += EquipSlot[i].STR_plus;
            DEF_plus += EquipSlot[i].DEF_plus;
            DEX_plus += EquipSlot[i].DEX_plus;
            INT_plus += EquipSlot[i].INT_plus;
            MaxHP_plus += EquipSlot[i].MaxHP_plus;
            MaxMP_plus += EquipSlot[i].MaxMP_plus;
            HpRecover_plus += EquipSlot[i].HpRecover_plus;
            MpRecover_plus += EquipSlot[i].MpRecover_plus;

        }


        AllStatUI.text =
            "STR\t+" + STR_plus + "\n" +
            "DEF\t+" + DEF_plus + "\n" +
            "DEX\t+" + DEX_plus + "\n" +
            "INT\t+" + INT_plus + "\n" +
            "MHP\t+" + MaxHP_plus + "\n" +
            "MMP\t+" + MaxMP_plus + "\n" +
            "HPR\t+" + HpRecover_plus + "\n" +
            "MPR\t+" + MpRecover_plus;

        PlayerManager.Instance.FinalStatUpdate(STR_plus, DEF_plus, DEX_plus, INT_plus, MaxHP_plus, MaxMP_plus, HpRecover_plus, MpRecover_plus);
    }


    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);

        transform.position = mousePosition + GAP;

    }

    public void OnPointerDown(PointerEventData eventData)
    {

        FirstMousePos = eventData.position;
        GAP = transform.position - FirstMousePos;
        gameObject.transform.SetAsLastSibling();

    }
}
