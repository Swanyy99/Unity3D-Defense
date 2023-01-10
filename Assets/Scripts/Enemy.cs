using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;

    private Rigidbody rigid;

    private Collider col;

    //public GameObject destination;

    private Animator anim;

    private int curWayIndex = 0;

    public TextMeshProUGUI Level;
    private int level;
    public int MaxHp;
    public int Hp;

    public CharacterController player;

    private float Distance;

    [SerializeField]
    private GameObject Weapon;

    //[SerializeField]
    //private GameObject DeathEffect;

    Vector3 moveVec;

    //private Rigidbody rigid;

    private void Awake()
    {
        //rigid = GetComponent<Rigidbody>();
        if (Type("Enemy"))  agent = GetComponent<NavMeshAgent>();
        if (Type("Boss"))   anim = GetComponent<Animator>();
        if (Type("Boss"))   rigid = GetComponent<Rigidbody>();
        if (Type("Boss"))   col = GetComponent<Collider>();
    }

    private void Start()
    {
        if (Type("Enemy"))
        {
            SetNextPoint();

            if (WaveManager.Instance.Wave < 10)
            {
                //MaxHp = WaveManager.Instance.Wave + 2;
                MaxHp = WaveManager.Instance.Wave + WaveManager.Instance.Wave + 3;
                agent.speed = 2.5f;
            }

            if (WaveManager.Instance.Wave >= 10)
            {
                MaxHp = WaveManager.Instance.Wave * 4;
                agent.speed = 3f;
            }
        }

        Hp = MaxHp;
        level = WaveManager.Instance.Wave;

        Level.text = "Lv. " + level.ToString();

    }

    private void Update()
    {
        if (Type("Enemy"))
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

    private bool IsArrive()
    {
        if ((WaveManager.Instance.WayPoints[curWayIndex].position - transform.position).sqrMagnitude < 0.1f)
            return true;
        else
            return false;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("WayPoint"))
    //    {
    //        if (curWayIndex >= WaveManager.Instance.WayPoints.Count - 1)
    //            OnArriveEndPoint();
    //        else
    //            SetNextPoint();
    //    }
    //}

    private void OnArriveEndPoint()
    {
        Debug.Log("으악");
        WaveManager.Instance.TakeDamage(1);
        Destroy(gameObject);
    }

    private void SetNextPoint()
    {
        curWayIndex++;
        agent.destination = WaveManager.Instance.WayPoints[curWayIndex].position;
        //destination.transform.position = WaveManager.Instance.WayPoints[curWayIndex].position;
    }

    public void TakeDamage(int damage)
    {
        Hp -= damage;

        if (Hp <= 0)
        {
            Hp = 0;
            BuildManager.Instance.GainEnergy(1);
            WaveManager.Instance.WaveMonsterDeath += 1;

            if (Type("Boss"))
            {
                Die();
            }
            else
                Destroy(gameObject);

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
        Weapon.transform.parent = Weapon.transform;
        StartCoroutine(Death());
    }

    //private void GoAway()
    //{
    //    if (Distance < 1f)
    //    {
    //        Debug.Log("보스가 당신을 밀어내고 있습니다.");
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

    private IEnumerator Death()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            //Instantiate(DeathEffect, transform.GetChild(0).position, transform.GetChild(0).rotation);
            Destroy(gameObject);
            break;
        }
    }

}