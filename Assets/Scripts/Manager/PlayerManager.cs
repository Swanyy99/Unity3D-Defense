using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using System;

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
    [SerializeField, Tooltip("캐릭터의 힘\nㄴ 기본공격 대미지와 스킬 대미지에 영향을 미칩니다.")]
    private int Str;
    [SerializeField, Tooltip("캐릭터의 민첩\nㄴ 크리티컬 확률과 이동속도에 영향을 미칩니다.")]
    private int Dex;
    [SerializeField, Tooltip("캐릭터의 지능\nㄴ 마나를 소모하는 모든 행동에 영향을 미칩니다.")]
    private int Int;
    [SerializeField, Tooltip("캐릭터의 방어력\nㄴ 피격 시 대미지 감소량에 영향을 미칩니다.")]
    private int Def;
    [SerializeField, Tooltip("캐릭터의 최대체력\nㄴ 내 최대체력에 영향을 미칩니다.")]
    private int MaxHp;
    [SerializeField, Tooltip("캐릭터의 최대마나\nㄴ 내 최대마나에 영향을 미칩니다.")]
    private int MaxMp;
    [SerializeField, Tooltip("캐릭터의 체력회복량\nㄴ 3초마다 자동 회복되는 HP량에 영향을 미칩니다.")]
    private int hpRecover;
    [SerializeField, Tooltip("캐릭터의 마나회복량\nㄴ 1초마다 자동 회복되는 MP량에 영향을 미칩니다.")]
    private int mpRecover;

    [Header("Effect")]
    [SerializeField]
    private GameObject LevelUpEffect;

    public UnityAction<int> OnExpChanged;
    public UnityAction<int> OnMaxExpChanged;
    public UnityAction<int> OnMaxHpChanged;
    public UnityAction<int> OnCurHpChanged;
    public UnityAction<int> OnMaxMpChanged;
    public UnityAction<int> OnCurMpChanged;

    public UnityAction<int> OnCurStrChanged;

    private GameObject player;

    public int STR
    {
        get { return Str; }
        private set { Str = value; OnCurStrChanged?.Invoke(Str); }
    }
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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RecoverHP());
        StartCoroutine(RecoverMP());
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        if (HP - damage > 0)
        {
            LogManager.Instance.logText.text += "<#DC143C>[알림]</color><#FFFFFF></color> " + damage + " 의 대미지를 받았습니다. \n";
            LogManager.Instance.StartCoroutine(LogManager.Instance.updateScroll());
            HP -= damage;
        }
        else
        {
            LogManager.Instance.logText.text += "<#DC143C>[알림]</color><#FFFFFF></color> " + damage + " 의 대미지를 받고 <#DC143C>사망</color><#FFFFFF></color>했습니다. \n";
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


        // TODO : if (Heart <= 0) GameManager.Instance.GameOver();
    }

    public void GainHp(int hp)
    {

        if (HP + hp >= maxHp)
            HP = maxHp;
        else
            HP += hp;


        // TODO : if (Heart <= 0) GameManager.Instance.GameOver();
    }

    public void GainMp(int mp)
    {

        if (MP + mp >= maxMp)
            MP = maxMp;
        else
            MP += mp;


        // TODO : if (Heart <= 0) GameManager.Instance.GameOver();
    }

    public void GainExp(int xp)
    {

        
           


        this.EXP += xp;
        LogManager.Instance.logText.text += "<#32CD32>[알림]</color><#FFFFFF></color> " + xp + " 경험치 획득\n";
        LogManager.Instance.StartCoroutine(LogManager.Instance.updateScroll());

        if (EXP >= maxExp)
        {
            EXP -= MaxEXP;
            MaxEXP = (int)(MaxEXP + MaxEXP * 1 / 2);
            level += 1;
            LevelUI.text = level.ToString();
            LogManager.Instance.logText.text += "<#32CD32>[알림]</color><#FFFFFF></color> 레벨 업! <#00FF7F>" + level + " 레벨</color><#FFFFFF></color> 이 되었습니다!\n";
            LogManager.Instance.StartCoroutine(LogManager.Instance.updateScroll());
            HP = MAXHP;
            MP = MAXMP;
            Instantiate(LevelUpEffect, player.transform.position, player.transform.rotation);

        }

        

        if (EXP == 0)
            ExpPercentageUI.text = "0 %";

        else

        {
            Debug.Log("경험치 퍼센트: " + (float) exp / maxExp * 100 + " %" );
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
