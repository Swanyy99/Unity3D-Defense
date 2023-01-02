using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    private Enemy target;

    private BoxCollider col;

    private Animator anim;

    [SerializeField]
    private GameObject hitEffect;

    [SerializeField]
    private GameObject hitPos;
    private void Start()
    {
        col = GetComponent<BoxCollider>();
        anim = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (OnBasicAttack())
        {
            if (other.gameObject.tag.Equals("Enemy"))
            {
                Debug.Log("기본공격 적중");
                target = other.GetComponent<Enemy>();
                Instantiate(hitEffect, hitPos.transform.position, hitPos.transform.rotation);
                if (curAnim("comboSlash4"))
                    target.TakeDamage(3);
                else
                    target.TakeDamage(1);
            }
        }
    }

    bool curAnim(string name)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    bool OnBasicAttack()
    {
        if (curAnim("comboSlash1") == true ||
            curAnim("comboSlash2") == true ||
            curAnim("comboSlash3") == true ||
            curAnim("comboSlash4") == true)
            return true;
        else
            return false;
    }
}
