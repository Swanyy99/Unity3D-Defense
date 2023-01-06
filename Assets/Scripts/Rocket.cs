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
    private int RocketDamage;

    [SerializeField]
    private GameObject BoomEffect;

    private Enemy Mytarget;

    private Enemy target;

    private Coroutine autoDestoryRoutine;

    [SerializeField]
    private bool isGlobalAttack;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        Mytarget = GameManager.Instance.target;
        autoDestoryRoutine = StartCoroutine(AutoDestoryRoutine());
    }

    private void Update()
    {
       
        if (Mytarget != null)
        {
            var direction = (Mytarget.transform.position - transform.position).normalized;
            rigid.velocity = transform.forward * RocketSpeed;
            var targetRotation = Quaternion.LookRotation(direction);
            rigid.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, 10f));
        }

        if (Mytarget == null)
        {
            Mytarget = GameManager.Instance.target;
        }
    }

    private IEnumerator AutoDestoryRoutine()
    {
        yield return new WaitForSeconds(3f / Time.timeScale);
        Instantiate(BoomEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            Debug.Log("¹Ì»çÀÏ ºÎµúÈû");
            Instantiate(BoomEffect, transform.position, transform.rotation);
            target = other.GetComponent<Enemy>();

            if (isGlobalAttack == false)
                target.TakeDamage(RocketDamage);

            Destroy(gameObject);
        }

        if (other.gameObject.tag.Equals("Boss"))
        {
            Debug.Log("¹Ì»çÀÏ ºÎµúÈû");
            Instantiate(BoomEffect, transform.position, transform.rotation);
            target = other.GetComponent<Enemy>();

            if (isGlobalAttack == false)
                target.TakeDamage(RocketDamage);

            Destroy(gameObject);
        }
    }

}
