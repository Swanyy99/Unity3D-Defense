using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    private Enemy target;

    private BoxCollider col;

    private Animator anim;

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
                Debug.Log("적과 충돌");
                target = other.GetComponent<Enemy>();
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
