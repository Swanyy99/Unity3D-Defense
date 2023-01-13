using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : SingleTon<PlayerManager>
{
    [Header("Basic")]
    [SerializeField]
    private int level;
    [SerializeField]
    private int exp;
    [SerializeField]
    private int hp;
    [SerializeField]
    private int maxHp;
    [SerializeField]
    private int mp;
    [SerializeField]
    private int maxMp;

    [Header("Stat")]
    [SerializeField]
    private int str;
    [SerializeField]
    private int def;
    [SerializeField]
    private int hpRecover;
    [SerializeField]
    private int mpRecover;

    public UnityAction<int> OnExpChanged;
    public UnityAction<int> OnMaxHpChanged;
    public UnityAction<int> OnCurHpChanged;
    public UnityAction<int> OnMaxMpChanged;
    public UnityAction<int> OnCurMpChanged;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        if (HP - damage > 0)
            HP -= damage;
        else
            HP = 0;

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
