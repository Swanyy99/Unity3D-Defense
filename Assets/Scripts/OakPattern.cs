using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class OakPattern : MonoBehaviour
{
    [SerializeField]
    private GameObject AttackVFX;

    [SerializeField]
    private Transform AttackPos;

    [SerializeField]
    private GameObject oakFinger1;
    [SerializeField]
    private GameObject oakFinger2;
    [SerializeField]
    private GameObject oakFinger3;
    [SerializeField]
    private GameObject oakFinger4;
    [SerializeField]
    private GameObject oakFinger5;


    private Animator anim;

    private bool attack1time;

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        if (curAnim("Attack"))
        {
            oakFinger1.SetActive(true);
            oakFinger2.SetActive(true);
            oakFinger3.SetActive(true);
            oakFinger4.SetActive(true);
            oakFinger5.SetActive(true);
        }
        else
        {
            oakFinger1.SetActive(false);
            oakFinger2.SetActive(false);
            oakFinger3.SetActive(false);
            oakFinger4.SetActive(false);
            oakFinger5.SetActive(false);
        }


        if (curAnim("Attack") && attack1time == false)
        {
            StartCoroutine(Attack());
            attack1time = true;
        }

        if (curAnim("Move") || curAnim("Idle"))
            attack1time = false;
    }


    bool curAnim(string name)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }


    public IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (gameObject == null)
                break;
            GameObject att = Instantiate(AttackVFX, AttackPos.transform.position, AttackPos.transform.rotation);
            att.transform.parent = this.transform;
            break;
        }
    }

}

