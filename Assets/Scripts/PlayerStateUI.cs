using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStateUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI LevelUI;
    [SerializeField]
    private TextMeshProUGUI ExpUI;
    [SerializeField]
    private TextMeshProUGUI HpUI;
    [SerializeField]
    private TextMeshProUGUI MpUI;
    [SerializeField]
    private TextMeshProUGUI GoldUI;

    [Header("Bar")]
    [SerializeField]
    private GameObject HPBAR;
    [SerializeField]
    private GameObject MPBAR;


    private List<GameObject> Obj;

    private void Start()
    {
        PlayerManager.Instance.OnCurHpChanged += ChangeCurHp;
        PlayerManager.Instance.OnCurMpChanged += ChangeCurMp;
        PlayerManager.Instance.OnExpChanged += GainEXP;
        PlayerManager.Instance.OnMaxExpChanged += ChangeMaxEXP;
        BuildManager.Instance.OnChangeGold += ChangeGold;

        ChangeCurHp(PlayerManager.Instance.HP);
        GainEXP(PlayerManager.Instance.EXP);
        ChangeMaxEXP(PlayerManager.Instance.MaxEXP);
        ChangeCurMp(PlayerManager.Instance.MP);
        ChangeGold(BuildManager.Instance.Gold);
    }

    public void ChangeCurHp(int hp)
    {
        

        float v = Mhp() / 14f;
        int count = 0;
        float h = hp / v;
        if (h > 14)
            h = 14;
        //if (hp % v > 0)
        //    h++;


        for (int i = 0; i < (int)h; i++) 
        {
            HPBAR.transform.GetChild(i).gameObject.SetActive(true);
            count++;
        }

        for (int i = 0; i < 14 - count; i++) 
        {
            HPBAR.transform.GetChild(count + i).gameObject.SetActive(false);
        }

        if (hp > Mhp())
        {
            hp = Mhp();
        }

        HpUI.text = hp.ToString();
    }

    public void ChangeCurMp(int mp)
    {


        float v = Mmp() / 14f;
        int count = 0;
        float h = mp / v;
        if (h > 14)
            h = 14;
        


        for (int i = 0; i < (int)h; i++)
        {
            MPBAR.transform.GetChild(i).gameObject.SetActive(true);
            count++;
        }

        for (int i = 0; i < 14 - count; i++)
        {
            MPBAR.transform.GetChild(count + i).gameObject.SetActive(false);
        }

        if (mp > Mmp())
        {
            mp = Mmp();
        }

        MpUI.text = mp.ToString();
    }

    public void GainEXP(int exp)
    {
        //ExpUI.text = (PlayerManager.Instance.EXP / PlayerManager.Instance.MaxEXP * 100).ToString();
            
        //exp.ToString();
    }

    public void ChangeMaxEXP(int MaxEXP)
    {
        //ExpUI.text = (PlayerManager.Instance.EXP / PlayerManager.Instance.MaxEXP * 100).ToString();
    }

    public void ChangeGold(int gold)
    {
        GoldUI.text = gold.ToString() + " G";
    }

    private float nowHP()
    {
        return PlayerManager.Instance.HP / PlayerManager.Instance.MAXHP * 1.4f;
    }

    private int Mhp()
    {
        return PlayerManager.Instance.MAXHP;
    }

    private int Mmp()
    {
        return PlayerManager.Instance.MAXMP;
    }

    private bool CheckHP(float a, float b)
    {
        return a < nowHP() && nowHP() > b;
    }
}