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

    private Enemy Mytarget;

    private Enemy target;

    private Coroutine autoDestoryRoutine;

    private Tower tower;

    private int MaxTarget;
    

    [SerializeField]
    private bool isGlobalAttack;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        tower = GetComponentInParent<Tower>();
        Mytarget = GameManager.Instance.target;
        autoDestoryRoutine = StartCoroutine(AutoDestoryRoutine());
    }

    private void Update()
    {
       
        if (Mytarget != null)
        {
            var direction = (Mytarget.transform.GetChild(0).position - transform.position).normalized;
            rigid.velocity = transform.forward * RocketSpeed;
            var targetRotation = Quaternion.LookRotation(direction);
            rigid.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, RotateSpeed));
        }

        if (Mytarget == null)
        {
            Mytarget = GameManager.Instance.target;
        }
    }

    private IEnumerator AutoDestoryRoutine()
    {
        yield return new WaitForSeconds(3f);
        GameObject temp = Instantiate(BoomEffect, transform.position, transform.rotation);
        temp.transform.parent = this.transform.parent.transform;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            MaxTarget += 1;
            Debug.Log("¹Ì»çÀÏ ºÎµúÈû");

            if (MaxTarget <= 1)
            {
                GameObject temp = Instantiate(BoomEffect, transform.position, transform.rotation);
                temp.transform.parent = this.transform.parent.transform;
            }

            target = other.GetComponent<Enemy>();

            if (isGlobalAttack == false)
            {
                if (MaxTarget <= 1)
                {
                    target.TakeDamage(tower.damage);
                    tower.durability -= 1;
                }
            }

            Destroy(gameObject);
            //gameObject.SetActive(false);
        }

        if (other.gameObject.tag.Equals("Boss"))
        {
            MaxTarget += 1;
            Debug.Log("¹Ì»çÀÏ ºÎµúÈû");

            if (MaxTarget <= 1)
            {
                GameObject temp = Instantiate(BoomEffect, transform.position, transform.rotation);
                temp.transform.parent = this.transform.parent.transform;
            }

            target = other.GetComponent<Enemy>();

            if (isGlobalAttack == false)
            {
                if (MaxTarget <= 1)
                {
                    target.TakeDamage(tower.damage);
                    tower.durability -= 1;
                }
            }

            //gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

}
