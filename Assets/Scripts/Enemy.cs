using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using Unity.Mathematics;
using System;
using Random = UnityEngine.Random;
using System.Security.Cryptography;
using ObjectPool;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;

    private Rigidbody rigid;

    private Collider col;

    //public GameObject destination;

    private Animator anim;

    private bool PlayerDetect;

    private int curWayIndex = 0;
    
    private int level;

    private GameObject player;
    private GameObject MainGate;

    [Header("Spec")]
    public int Exp;
    public int MaxHp;
    public int Hp;
    [SerializeField]
    private int damage;


    private float distance;
    private float distance2;

    private float AttackTimer;
    private float GateAttackTimer;
    private float StopTimer;


    private Vector3 playerpos;

    [Header("Component")]
    public TextMeshProUGUI Level;
    [SerializeField]
    private GameObject Standard;
    [SerializeField]
    private GameObject damageTextPos;
    [SerializeField]
    private GameObject Weapon;
    [SerializeField]
    private GameObject expVFX;
    [SerializeField]
    private GameObject floatingDamage;

    private PoolableObject pool;
    public int Damage { get { return damage; } private set { damage = value; } }

    bool isDead;
    [Header("is Damagble?")]
    public bool Damagable = true;

    [Serializable]
    struct dropItemList
    {
        public string Name;
        public GameObject item;
        public float Chance;
    }

    [Space]
    [SerializeField]
    private List<dropItemList> DropItemList;

    private void OnEnable()
    {
        EnemyInit();
    }


    private void Awake()
    {
        if (Type("Enemy")) agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        MoveRoad();
        DetectPlayer();
    }

    private bool IsArrive()
    {
        if ((WaveManager.Instance.WayPoints[curWayIndex].position - transform.position).sqrMagnitude < 0.2f)
            return true;
        else
            return false;
    }

    private void MoveRoad()
    {
        if (Type("Enemy") && PlayerDetect == false)
        {
            if (IsArrive())
            {
                if (curWayIndex == WaveManager.Instance.WayPoints.Count - 1)
                    OnArriveEndPoint();

                else
                    SetNextPoint();
            }
        }
    }

    private void DetectPlayer()
    {
        if (Type("Enemy"))
        {
            if (curAnim("Attack"))
            {
                anim.SetBool("Move", false);
                agent.SetDestination(gameObject.transform.position);
            }

            AttackTimer += Time.deltaTime;
            distance = Vector3.Distance(player.transform.position, gameObject.transform.position);
            if (distance <= 1.2f)
            {
                playerpos = new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z);
                if (!curAnim("Attack"))
                {
                    gameObject.transform.LookAt(playerpos);
                }
                PlayerDetect = true;
                anim.SetBool("Move", false);
                agent.SetDestination(gameObject.transform.position);

                if (AttackTimer >= 4)
                {
                    anim.SetBool("Move", false);
                    anim.SetTrigger("Attack");
                    AttackTimer = 0;
                }
            }

            else if (distance > 1.2f && distance < 3.5f && !curAnim("Attack"))
            {
                anim.SetBool("Move", true);
                agent.SetDestination(player.transform.position);
                PlayerDetect = true;
            }

            else if (distance >= 3.5f && PlayerDetect == true && !curAnim("Attack"))
            {
                PlayerDetect = false;
                anim.ResetTrigger("Attack");
                anim.SetBool("Move", true);
                curWayIndex = WaveManager.Instance.WayPoints.Count - 1;
                agent.destination = WaveManager.Instance.WayPoints[WaveManager.Instance.WayPoints.Count - 1].position;

            }
        }
    }


    private void OnArriveEndPoint()
    {
        Debug.Log("으악");
        PlayerManager.Instance.TakeDamage(damage * 2);
        pool.Return();
    }

    private void SetNextPoint()
    {
        if (Type("Enemy") && agent.enabled == true)
        {
            if (curWayIndex < WaveManager.Instance.WayPoints.Count)
            {
                curWayIndex++;
                agent.destination = WaveManager.Instance.WayPoints[curWayIndex].position;
            }
            else
            {
                agent.destination = WaveManager.Instance.WayPoints[curWayIndex].position;
            }
        }
    }

    public void GainHP(int hp)
    {
        Hp += hp;
    }

    public void RecoverHpBoss()
    {
        Hp += MaxHp * 2 / 10;

        if (Hp > MaxHp)
            Hp = MaxHp;
    }

    public void TakeDamage(int damage)
    {
        int DAMAGE;
        bool critical = Critical.CriticalAttack(PlayerManager.Instance.DEX);

        DAMAGE = critical ? (int)(damage * 1.5f) : damage;

        if (Type("Enemy")) Hp -= DAMAGE;

        else if (Type("Boss"))
        {
            if (Damagable == true)
            {
                Hp -= DAMAGE;
            }
            else
            {
                DAMAGE = 0;
                Hp -= DAMAGE;
            }
        }

        GameObject Damageinstance = PoolManager.Instance.Get(floatingDamage, damageTextPos.transform.position, damageTextPos.transform.rotation);

        Damageinstance.GetComponent<FloatingDamage>().Init(DAMAGE, critical);

        if (Damageinstance == null)
            return;

        if (Hp <= 0)
        {
            Hp = 0;

            if (Type("Boss"))
            {
                Die();
            }

            else
            {
                if (isDead == false)
                {
                    dropItem();
                    GameObject instance = PoolManager.Instance.Get(expVFX, transform.position, this.transform.rotation, this.transform);
                    isDead = true;
                    if (instance == null)
                        return;
                }

                pool.StartCoroutine(pool.DelayToReturn());
            }

        }
    }

    public void Die()
    {
        anim.SetTrigger("Death");
        rigid.useGravity = false;
        col.enabled = false;

        bool drop = Random.Range(0, 2) == 0;

        if (drop)
        {
            Weapon.GetComponent<Rigidbody>().useGravity = true;
            Weapon.GetComponent<Rigidbody>().isKinematic = false;
            Weapon.GetComponent<Collider>().isTrigger = false;
            Weapon.GetComponent<InteractionAdaptor>().enabled = true;
            Weapon.GetComponent<Item>().enabled = true;
            Weapon.transform.parent = null;
        }
        dropItem();
        StartCoroutine(Death());
    }

    public void dropItem()
    {
        for (int i = 0; i < DropItemList.Count; i++)
        {
            bool Itemdrop = Critical.RandomChance(DropItemList[i].Chance);
            float DropRandomRange = UnityEngine.Random.Range(-0.6f, 0.6f);

            Vector3 randomPos = new Vector3(transform.position.x + DropRandomRange, transform.position.y, transform.position.z + DropRandomRange);
            if (Itemdrop)
            {
                if (DropItemList[i].Name == "오크의 살점")
                {
                    GameObject instance = PoolManager.Instance.Get(DropItemList[i].item, randomPos, transform.rotation);
                    if (instance == null)
                        return;
                }
                else
                {
                    Instantiate(DropItemList[i].item, randomPos, transform.rotation);
                }

            }
        }

    }

    private void EnemyInit()
    {
        isDead = false;
        curWayIndex = 0;
        anim = GetComponent<Animator>();
        if (Type("Boss")) rigid = GetComponent<Rigidbody>();
        if (Type("Boss")) col = GetComponent<Collider>();
        if (Type("Enemy")) pool = GetComponent<PoolableObject>();


        player = GameObject.Find("Player");
        MainGate = GameObject.Find("MainGate");


        if (Type("Enemy"))
        {
            SetNextPoint();
            anim.SetBool("Move", true);
            if (NowWave(1, 5))
            {
                MaxHp = WaveManager.Instance.Wave + WaveManager.Instance.Wave + 3;
                agent.speed = 2.5f;
                damage = WaveManager.Instance.Wave + 4;
            }

            else if (NowWave(6, 10))
            {
                MaxHp = WaveManager.Instance.Wave * 7;
                damage = WaveManager.Instance.Wave * 3;
                agent.speed = 3f;
            }

            else
            {
                MaxHp = WaveManager.Instance.Wave * 10;
                damage = WaveManager.Instance.Wave * 6;
                agent.speed = 3f;
            }

            if (NowWave(1, 4))
            {
                Exp = 1;
            }
            else if (NowWave(5, 7))
            {
                Exp = 2;
            }
            else if (NowWave(8, 10))
            {
                Exp = 3;
            }
            else
            {
                Exp = 5;
            }
        }

        if (Type("Boss"))
        {
            MaxHp = WaveManager.Instance.Wave * 200;
            damage = WaveManager.Instance.Wave * 10;
            Exp = 70 * WaveManager.Instance.Wave / 5;
        }

        Hp = MaxHp;
        level = WaveManager.Instance.Wave;

        Level.text = "Lv. " + level.ToString();
    }

    private bool Type(string name)
    {
        return gameObject.tag.Equals(name);
    }

    bool curAnim(string name)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    bool NowWave(int a, int b)
    {
        if (WaveManager.Instance.Wave >= a && WaveManager.Instance.Wave <= b)
            return true;
        else return false;
    }

    private IEnumerator Death()
    {
        
        yield return new WaitForSeconds(3f);
        GameObject instance = PoolManager.Instance.Get(expVFX, transform.position, this.transform.rotation, this.transform);
        isDead = true;
        if (instance == null) yield break;
        StartCoroutine(DelayBossDeath());
    }

    private IEnumerator DelayBossDeath()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);

    }

    private void OnDisable()
    {
        if (Type("Enemy"))
        {
            agent.enabled = false;
            PlayerDetect = false;

            //curWayIndex = 0;
        }

        WaveManager.Instance.WaveMonsterDeath += 1;

    }

}