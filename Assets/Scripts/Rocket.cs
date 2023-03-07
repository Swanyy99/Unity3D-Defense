using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rocket : MonoBehaviour
{
    private Rigidbody rigid;

    [SerializeField]
    private float RocketSpeed;
    [SerializeField]
    private float RotateSpeed;

    [SerializeField]
    private int RocketDamage;

    [SerializeField]
    private GameObject BoomEffect;

    public Enemy Mytarget;

    private Enemy target;

    private Enemy NewTarget;

    private Coroutine autoDestoryRoutine;

    private Tower tower;

    private int MaxTarget;

    public LayerMask targetMask;

    public int Damage;


    [SerializeField]
    private bool isGlobalAttack;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        tower = GetComponentInParent<Tower>();
        Mytarget = tower.target;
        Damage = tower.damage;
    }
    private void Start()
    {
        this.transform.parent = null;
        //Mytarget = GameManager.Instance.target;
        autoDestoryRoutine = StartCoroutine(AutoDestoryRoutine());
    }

    private void Update()
    {
        FlyToTarget();
    }

    private void FlyToTarget()
    {
        if (Mytarget != null && Mytarget.isActiveAndEnabled)
        {
            var direction = (Mytarget.transform.GetChild(0).position - transform.position).normalized;
            rigid.velocity = transform.forward * RocketSpeed;
            var targetRotation = Quaternion.LookRotation(direction);
            rigid.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, RotateSpeed));
        }

        else
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 3f, targetMask);
            for (int i = 0; i < colliders.Length; i++)
            {
                NewTarget = colliders[i].GetComponent<Enemy>();
                if (null != NewTarget)
                {
                    Mytarget = NewTarget;
                    break;
                }
            }
        }
    }

    private IEnumerator AutoDestoryRoutine()
    {
        yield return new WaitForSeconds(2f);
        GameObject temp = Instantiate(BoomEffect, transform.position, transform.rotation, this.transform);
        StartCoroutine(DelayDestroy());
    }

    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(0.05f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            MaxTarget += 1;
            //Debug.Log("¹Ì»çÀÏ ºÎµúÈû");

            if (MaxTarget <= 1)
            {
                GameObject temp = Instantiate(BoomEffect, transform.position, transform.rotation, gameObject.transform);
                tower.durability -= 1;

            }

            target = other.GetComponent<Enemy>();

            if (isGlobalAttack == false)
            {
                if (MaxTarget <= 1)
                {
                    target.TakeDamage(tower.damage);
                }
            }

            StartCoroutine(DelayDestroy());
            //Destroy(gameObject);
            //gameObject.SetActive(false);
        }

        if (other.gameObject.tag.Equals("Boss"))
        {
            MaxTarget += 1;

            if (MaxTarget <= 1)
            {
                GameObject temp = Instantiate(BoomEffect, transform.position, transform.rotation, gameObject.transform);
                tower.durability -= 1;

            }

            target = other.GetComponent<Enemy>();

            if (isGlobalAttack == false)
            {
                if (MaxTarget <= 1)
                {
                    target.TakeDamage(tower.damage);
                }
            }

            StartCoroutine(DelayDestroy());
            //gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }

}
