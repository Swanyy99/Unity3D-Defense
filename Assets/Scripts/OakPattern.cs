using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;

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
    [Header("DropItem")]
    [SerializeField]
    private GameObject dropItem1;
    [SerializeField]
    private GameObject dropItem2;
    [SerializeField]
    private GameObject dropItem3;
    [SerializeField]
    private GameObject dropItem4;
    [SerializeField]
    private GameObject dropItem5;
    [SerializeField]
    private GameObject dropItem6;
    [SerializeField]
    private GameObject dropItem7;
    [SerializeField]
    private GameObject dropItem8;
    [SerializeField]
    private GameObject dropItem9;
    [SerializeField]
    private GameObject dropItem10;
    [SerializeField]
    private GameObject dropItem11;
    [SerializeField]
    private GameObject dropItem12;

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

    public void dropItem()
    {
        int a = UnityEngine.Random.Range(0, 15);
        switch (a)
        {
            case 0:
                GameObject instance = PoolManager.Instance.Get(dropItem1, transform.position, transform.rotation);
                if (instance == null)
                    return;
                //instance.transform.position = transform.position;
                //instance.transform.rotation = transform.rotation;
                //Instantiate(dropItem1, transform.position, transform.rotation);
                break;

            case 1:
                GameObject instance2 = PoolManager.Instance.Get(dropItem2, transform.position, transform.rotation);
                if (instance2 == null)
                    return;
                //instance2.transform.position = transform.position;
                //instance2.transform.rotation = transform.rotation;
                //Instantiate(dropItem2, transform.position, transform.rotation);
                break;

            case 2:
                GameObject instance3 = PoolManager.Instance.Get(dropItem3, transform.position, transform.rotation);
                if (instance3 == null)
                    return;
                //instance3.transform.position = transform.position;
                //instance3.transform.rotation = transform.rotation;
                //Instantiate(dropItem3, transform.position, transform.rotation);
                break;

            case 3:
                GameObject instance4 = PoolManager.Instance.Get(dropItem4, transform.position, transform.rotation);
                if (instance4 == null)
                    return;
                break;

            case 4:
                Instantiate(dropItem5, transform.position, transform.rotation);
                break;

            case 5:
                Instantiate(dropItem6, transform.position, transform.rotation);
                break;
            case 6:
                Instantiate(dropItem7, transform.position, transform.rotation);
                break;
            case 7:
                Instantiate(dropItem8, transform.position, transform.rotation);
                break;
            case 8:
                Instantiate(dropItem9, transform.position, transform.rotation);
                break;
            case 9:
                Instantiate(dropItem10, transform.position, transform.rotation);
                break;
            case 10:
                Instantiate(dropItem11, transform.position, transform.rotation);
                break;
            case 11:
                Instantiate(dropItem12, transform.position, transform.rotation);
                break;

            default:
                break;

        }
    }
}

