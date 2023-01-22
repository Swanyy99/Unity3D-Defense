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
    [SerializeField, Tooltip("ĳ������ ��\n�� �⺻���� ������� ��ų ������� ������ ��Ĩ�ϴ�.")]
    private int Str;
    [SerializeField, Tooltip("ĳ������ ��ø\n�� ũ��Ƽ�� Ȯ���� �̵��ӵ��� ������ ��Ĩ�ϴ�.")]
    private int Dex;
    [SerializeField, Tooltip("ĳ������ ����\n�� ������ �Ҹ��ϴ� ��� �ൿ�� ������ ��Ĩ�ϴ�.")]
    private int Int;
    [SerializeField, Tooltip("ĳ������ ����\n�� �ǰ� �� ����� ���ҷ��� ������ ��Ĩ�ϴ�.")]
    private int Def;
    [SerializeField, Tooltip("ĳ������ �ִ�ü��\n�� �� �ִ�ü�¿� ������ ��Ĩ�ϴ�.")]
    private int MaxHp;
    [SerializeField, Tooltip("ĳ������ �ִ븶��\n�� �� �ִ븶���� ������ ��Ĩ�ϴ�.")]
    private int MaxMp;
    [SerializeField, Tooltip("ĳ������ ü��ȸ����\n�� 3�ʸ��� �ڵ� ȸ���Ǵ� HP���� ������ ��Ĩ�ϴ�.")]
    private int hpRecover;
    [SerializeField, Tooltip("ĳ������ ����ȸ����\n�� 1�ʸ��� �ڵ� ȸ���Ǵ� MP���� ������ ��Ĩ�ϴ�.")]
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
            LogManager.Instance.logText.text += "<#DC143C>[�˸�]</color><#FFFFFF></color> " + damage + " �� ������� �޾ҽ��ϴ�. \n";
            LogManager.Instance.StartCoroutine(LogManager.Instance.updateScroll());
            HP -= damage;
        }
        else
        {
            LogManager.Instance.logText.text += "<#DC143C>[�˸�]</color><#FFFFFF></color> " + damage + " �� ������� �ް� <#DC143C>���</color><#FFFFFF></color>�߽��ϴ�. \n";
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
