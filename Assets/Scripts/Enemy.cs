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

    public TextMeshProUGUI Level;
    private int level;
    public int Exp;
    public int MaxHp;
    public int Hp;
    [SerializeField]
    private int damage;

    public GameObject player;
    public GameObject MainGate;

    private float distance;
    private float distance2;

    private float AttackTimer;
    private float GateAttackTimer;
    private float StopTimer;


    private Vector3 playerpos;

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

    private OakPattern oak;

    bool isDead;

    public bool Damagable = true;

    Vector3 moveVec;


    public int Damage { get { return damage; } private set { damage = value; } }

    

    private void OnEnable()
    {
        isDead = false;
        curWayIndex = 0;
        anim = GetComponent<Animator>();
        if (Type("Boss")) rigid = GetComponent<Rigidbody>();
        if (Type("Boss")) col = GetComponent<Collider>();
        if (Type("Enemy")) pool = GetComponent<PoolableObject>();
        if (Type("Enemy")) oak = GetComponent<OakPattern>();


        player = GameObject.Find("Player");
        MainGate = GameObject.Find("MainGate");


        if (Type("Enemy"))
        {
            SetNextPoint();
            anim.SetBool("Move", true);
            if (NowWave(1,5))
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
        Debug.Log("����");
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
        //destination.transform.position = WaveManager.Instance.WayPoints[curWayIndex].position;
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


        if (critical)
        {
            DAMAGE = (int)(damage * 1.5f);
        }
        else
        {
            DAMAGE = damage;
        }


        if (Type("Enemy"))
        {
            Hp -= DAMAGE;
        }

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
        if (damage != DAMAGE && Damagable == true)
            Damageinstance.GetComponent<TextMeshPro>().color = Color.red;
        else
            Damageinstance.GetComponent<TextMeshPro>().color = Color.white;

        Damageinstance.GetComponent<TextMeshPro>().text = DAMAGE.ToString();

        if (Damageinstance == null)
            return;

        if (Hp <= 0)
        {
            Hp = 0;
            //BuildManager.Instance.GainEnergy(1);

            if (Type("Boss"))
            {
                Die();
            }

            else
            {
                //int a = Random.Range(0, 90);
                //int b = Random.Range(0, 360);
                //int c = Random.Range(-90, 90);
                //quaternion random = quaternion.Euler(-a, b, c);
                //GameObject temp = Instantiate(expVFX, Standard.transform.position, random);
                //temp.transform.parent = this.transform;
                //Debug.Log("exp ��������");


                if (isDead == false)
                {
                    oak.dropItem();
                    GameObject instance = PoolManager.Instance.Get(expVFX, transform.position, this.transform.rotation, this.transform);
                    isDead = true;
                    if (instance == null)
                        return;
                }

                pool.StartCoroutine(pool.DelayToReturn());
                //Destroy(gameObject, 0.1f);
            }

        }
    }

    public void Die()
    {
        anim.SetTrigger("Death");
        rigid.useGravity = false;
        col.enabled = false;
        Weapon.GetComponent<Rigidbody>().useGravity = true;
        Weapon.GetComponent<Rigidbody>().isKinematic = false;
        Weapon.GetComponent<Collider>().isTrigger = false;
        Weapon.transform.parent = null;
        StartCoroutine(Death());
    }

    //private void GoAway()
    //{
    //    if (Distance < 1f)
    //    {
    //        Debug.Log("������ ����� �о�� �ֽ��ϴ�.");
    //        Vector3 fowardVec = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
    //        Vector3 moveInput = Vector3.forward * 5f;
    //        moveVec = fowardVec * moveInput.z;
    //        player.Move(moveVec * Time.deltaTime);
    //    }
    //}

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