using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class StatUIscript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI StrUI;

    [SerializeField]
    private TextMeshProUGUI DefUI;

    [SerializeField]
    private TextMeshProUGUI DexUI;

    [SerializeField]
    private TextMeshProUGUI IntUI;

    [SerializeField]
    private TextMeshProUGUI MhpUI;

    [SerializeField]
    private TextMeshProUGUI MmpUI;

    [SerializeField]
    private TextMeshProUGUI HpRUI;

    [SerializeField]
    private TextMeshProUGUI MpRUI;

    private void Start()
    {

        PlayerManager.Instance.OnStrChanged += ChangeStr;
        PlayerManager.Instance.OnDefChanged += ChangeDef;
        PlayerManager.Instance.OnDexChanged += ChangeDex;
        PlayerManager.Instance.OnIntChanged += ChangeInt;

        PlayerManager.Instance.OnMaxHpStatChanged += ChangeMaxHpStat;
        PlayerManager.Instance.OnMaxMpStatChanged += ChangeMaxMpStat;

        PlayerManager.Instance.OnHpRecoverChanged += ChangeHpR;
        PlayerManager.Instance.OnMpRecoverChanged += ChangeMpR;

    }

    public void ChangeStr(int str)
    {
        StrUI.text = str.ToString();
    }

    public void ChangeDef(int def)
    {
        DefUI.text = def.ToString();
    }

    public void ChangeDex(int dex)
    {
        DexUI.text = dex.ToString();
    }

    public void ChangeInt(int Int)
    {
        IntUI.text = Int.ToString();
    }

    public void ChangeMaxHpStat(int mHp)
    {
        MhpUI.text = mHp.ToString();
    }

    public void ChangeMaxMpStat(int mMp)
    {
        MmpUI.text = mMp.ToString();
    }

    public void ChangeHpR(int hpR)
    {
        HpRUI.text = hpR.ToString();
    }

    public void ChangeMpR(int mpR)
    {
        MpRUI.text = mpR.ToString();
    }
}
