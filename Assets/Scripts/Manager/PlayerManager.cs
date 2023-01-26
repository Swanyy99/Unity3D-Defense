using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using System;
using static Unity.Burst.Intrinsics.X86.Avx;

public class PlayerManager : SingleTon<PlayerManager>
{
    [Header("Basic")]
    [SerializeField]
    private int level;
    [SerializeField]
    private TextMeshProUGUI LevelUI;
    [SerializeField]
    private int exp;
    [SerializeField]
    private int maxExp;
    private int ExpPercent;
    [SerializeField]
    private TextMeshProUGUI ExpPercentageUI;
    [SerializeField]
    private TextMeshProUGUI ExpPopupUI;
    [SerializeField]
    private int hp;
    [SerializeField]
    private int maxHp;
    [SerializeField]
    private int mp;
    [SerializeField]
    private int maxMp;

    [Header("Stat")]
    [SerializeField, Tooltip("ĳ������ ��\n�� �⺻���� ������� ��ų ������� ������ ��Ĩ�ϴ�.")]
    private int Str;
    [SerializeField, Tooltip("ĳ������ ����\n�� �ǰ� �� ����� ���ҷ��� ������ ��Ĩ�ϴ�.")]
    private int Def;
    [SerializeField, Tooltip("ĳ������ ��ø\n�� ũ��Ƽ�� Ȯ���� �̵��ӵ��� ������ ��Ĩ�ϴ�.")]
    private int Dex;
    [SerializeField, Tooltip("ĳ������ ����\n�� ������ �Ҹ��ϴ� ��� �ൿ�� ������ ��Ĩ�ϴ�.")]
    private int Int;
    [SerializeField, Tooltip("ĳ������ �ִ�ü��\n�� �� �ִ�ü�¿� ������ ��Ĩ�ϴ�.")]
    private int maxHpStat;
    [SerializeField, Tooltip("ĳ������ �ִ븶��\n�� �� �ִ븶���� ������ ��Ĩ�ϴ�.")]
    private int maxMpStat;
    [SerializeField, Tooltip("ĳ������ ü��ȸ����\n�� 3�ʸ��� �ڵ� ȸ���Ǵ� HP���� ������ ��Ĩ�ϴ�.")]
    private int hpRecover;
    [SerializeField, Tooltip("ĳ������ ����ȸ����\n�� 1�ʸ��� �ڵ� ȸ���Ǵ� MP���� ������ ��Ĩ�ϴ�.")]
    private int mpRecover;

    public int originSTR;
    public int originDEX;
    public int originDEF;
    public int originINT;
    public int originMAXHP = 100;
    public int originMAXMP = 100;
    public int originHPR = 1;
    public int originMPR = 1;
    
    [Header("Effect")]
    [SerializeField]
    private GameObject LevelUpEffect;

    public UnityAction<int> OnExpChanged;
    public UnityAction<int> OnMaxExpChanged;
    public UnityAction<int> OnMaxHpChanged;
    public UnityAction<int> OnCurHpChanged;
    public UnityAction<int> OnMaxMpChanged;
    public UnityAction<int> OnCurMpChanged;

    public UnityAction<int> OnStrChanged;
    public UnityAction<int> OnDefChanged;
    public UnityAction<int> OnDexChanged;
    public UnityAction<int> OnIntChanged;
    public UnityAction<int> OnHpRecoverChanged;
    public UnityAction<int> OnMpRecoverChanged;
    public UnityAction<int> OnMaxHpStatChanged;
    public UnityAction<int> OnMaxMpStatChanged;

    private GameObject player;

    [SerializeField]
    private EquipmentUI equip;

    

    public int HP
    {
        get { return hp; }
        private set { hp = value; OnCurHpChanged?.Invoke(hp); }
    }

    public int MAXHP
    {
        get { return maxHp; }
        private set { maxHp = value; OnMaxHpChanged?.Invoke(maxHp); }
    }

    public int EXP
    {
        get { return exp; }
        private set { exp = value; OnExpChanged?.Invoke(exp); }
    }

    public int MaxEXP
    {
        get { return maxExp; }
        private set { maxExp = value; OnMaxExpChanged?.Invoke(maxExp); }
    }

    public int MP
    {
        get { return mp; }
        private set { mp = value; OnCurMpChanged?.Invoke(mp); }
    }

    public int MAXMP
    {
        get { return maxMp; }
        private set { maxMp = value; OnMaxMpChanged?.Invoke(maxMp); }
    }

    ///////////////////////////////    STAT   //////////////////////////////////////////////////

    public int STR
    {
        get { return Str; }
        private set { Str = value; OnStrChanged?.Invoke(Str); }
    }

    public int DEF
    {
        get { return Def; }
        private set { Def = value; OnDefChanged?.Invoke(Def); }
    }

    public int DEX
    {
        get { return Dex; }
        private set { Dex = value; OnDexChanged?.Invoke(Dex); }
    }

    public int INT
    {
        get { return Int; }
        private set { Int = value; OnIntChanged?.Invoke(Int); }
    }

    public int MaxHpStat
    {
        get { return maxHpStat; }
        private set { maxHpStat = value; OnMaxHpStatChanged?.Invoke(maxHpStat);}
    }

    public int MaxMpStat
    {
        get { return maxMpStat; }
        private set { maxMpStat = value; OnMaxMpStatChanged?.Invoke(maxMpStat); }
    }

    public int HpRecover
    {
        get { return hpRecover; }
        private set { hpRecover = value; OnHpRecoverChanged?.Invoke(hpRecover); }
    }

    public int MpRecover
    {
        get { return mpRecover; }
        private set { mpRecover = value; OnMpRecoverChanged?.Invoke(mpRecover); }
    }

    void Start()
    {
        StartCoroutine(RecoverHP());
        StartCoroutine(RecoverMP());
        player = GameObject.Find("Player");
        StatUpdate();
    }


    public void TakeDamage(int damage)
    {
        int Damage = damage - DEF;
        if (Damage < 0)
            Damage = 0;

        if (HP - Damage > 0)
        {
            LogManager.Instance.logText.text += "<#DC143C>[�˸�]</color><#FFFFFF></color> " + Damage + " �� ������� �޾ҽ��ϴ�. \n";
            LogManager.Instance.StartCoroutine(LogManager.Instance.updateScroll());
            HP -= Damage;
        }
        else
        {
            LogManager.Instance.logText.text += "<#DC143C>[�˸�]</color><#FFFFFF></color> " + Damage + " �� ������� �ް� <#DC143C>���</color><#FFFFFF></color>�߽��ϴ�. \n";
            LogManager.Instance.StartCoroutine(LogManager.Instance.updateScroll());
            HP = 0;
        }

        // TODO : if (Heart <= 0) GameManager.Instance.GameOver();
    }

    public void UseMana(int manaCost)
    {
        
        if (MP + manaCost <= 0)
            MP = 0;
        else
            MP -= manaCost;

    }

    public void GainHp(int hp)
    {

        if (HP + hp >= maxHp)
            HP = maxHp;
        else
            HP += hp;

    }

    public void GainMp(int mp)
    {

        if (MP + mp >= maxMp)
            MP = maxMp;
        else
            MP += mp;

    }

    public void GainExp(int xp)
    {

        this.EXP += xp;
        LogManager.Instance.logText.text += "<#32CD32>[�˸�]</color><#FFFFFF></color> " + xp + " ����ġ ȹ��\n";
        LogManager.Instance.StartCoroutine(LogManager.Instance.updateScroll());

        if (EXP >= maxExp)
        {
            EXP -= MaxEXP;
            MaxEXP = (int)(MaxEXP + MaxEXP * 1 / 2);
            level += 1;
            LevelUI.text = level.ToString();
            LogManager.Instance.logText.text += "<#32CD32>[�˸�]</color><#FFFFFF></color> ���� ��! <#00FF7F>" + level + " ����</color><#FFFFFF></color> �� �Ǿ����ϴ�!\n";
            LogManager.Instance.StartCoroutine(LogManager.Instance.updateScroll());
            HP = MAXHP;
            MP = MAXMP;
            Instantiate(LevelUpEffect, player.transform.position, player.transform.rotation);

        }


        if (EXP == 0)
            ExpPercentageUI.text = "0 %";

        else

        {
            Debug.Log("����ġ �ۼ�Ʈ: " + (float) exp / maxExp * 100 + " %" );
            //Debug.Log((int)(exp / maxExp * 100));
            ExpPercentageUI.text = ((float)exp / maxExp * 100).ToString("F0") + " %";
        }

        string Exptext = exp.ToString();
        string maxExptext = maxExp.ToString();

        ExpPopupUI.text = "[ " + Exptext + " / " + maxExptext + " ]";

        //try
        //{
        //    ExpPercentageUI.text = (EXP / MaxEXP * 100).ToString() + " %";
        //}
        //catch (DivideByZeroException exception)
        //{
        //    Debug.Log(exception);
        //    ExpPercentageUI.text = "0 %";
        //}
        //finally
        //{
        //    ExpPercentageUI.text = (EXP / MaxEXP * 100).ToString() + " %";
        //}

        // TODO : if (Heart <= 0) GameManager.Instance.GameOver();
    }

    public void FinalStatUpdate(int str, int def, int dex, int Int, int mhp, int mmp, int hpr, int mpr)
    {
        STR = str + originSTR;
        DEF = def + originDEF;
        DEX = dex + originDEX;
        INT = Int + originINT;

        MaxHpStat = mhp + originMAXHP;
        MaxMpStat = mmp + originMAXMP;
        HpRecover = hpr + originHPR;
        MpRecover = mpr + originMPR;

        MAXHP = MaxHpStat;
        MAXMP = MaxMpStat;

    }

    public void StatUpdate()
    {
        STR = equip.STR_plus + originSTR;
        DEF = equip.DEF_plus + originDEF;
        DEX = equip.DEX_plus + originDEX;
        INT = equip.INT_plus + originINT;

        MaxHpStat = equip.MaxHP_plus + originMAXHP;
        MaxMpStat = equip.MaxMP_plus + originMAXMP;
        HpRecover = equip.HpRecover_plus + originHPR;
        MpRecover = equip.MpRecover_plus + originMPR;

        MAXHP = MaxHpStat;
        MAXMP = MaxMpStat;
    }


    private IEnumerator RecoverHP()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            GainHp(hpRecover);
        }
    }

    private IEnumerator RecoverMP()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            GainMp(mpRecover);
        }
    }
}

public static class Critical
{
    public static bool GetThisChanceResult(float Chance)
    {
        if (Chance < 0.0000001f)
        {
            Chance = 0.0000001f;
        }

        bool Success = false;
        int RandAccuracy = 10000000;
        float RandHitRange = Chance * RandAccuracy;
        int Rand = UnityEngine.Random.Range(1, RandAccuracy + 1);
        if (Rand <= RandHitRange)
        {
            Success = true;
        }
        return Success;
    }

    public static bool CriticalAttack(float Percentage_Chance)
    {
        if (Percentage_Chance < 0.0000001f)
        {
            Percentage_Chance = 0.0000001f;
        }

        Percentage_Chance = Percentage_Chance / 100;

        bool Success = false;
        int RandAccuracy = 10000000;
        float RandHitRange = Percentage_Chance * RandAccuracy;
        int Rand = UnityEngine.Random.Range(1, RandAccuracy + 1);
        if (Rand <= RandHitRange)
        {
            Success = true;
        }
        return Success;
    }

    public static bool RandomChance(float Percentage_Chance)
    {
        if (Percentage_Chance < 0.0000001f)
        {
            Percentage_Chance = 0.0000001f;
        }

        Percentage_Chance = Percentage_Chance / 100;

        bool Success = false;
        int RandAccuracy = 10000000;
        float RandHitRange = Percentage_Chance * RandAccuracy;
        int Rand = UnityEngine.Random.Range(1, RandAccuracy + 1);
        if (Rand <= RandHitRange)
        {
            Success = true;
        }
        return Success;
    }
}
