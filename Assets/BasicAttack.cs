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
                target = other.GetComponent<Enemy>();
                Debug.Log(target.gameObject.name + "을(를) 공격했습니다.");
                Instantiate(hitEffect, hitPos.transform.position, hitPos.transform.rotation);
                if (curAnim("comboSlash1"))
                {
                    target.TakeDamage(1 + PlayerManager.Instance.STR) ;
                }
                else if (curAnim("comboSlash2"))
                {
                    target.TakeDamage(1 + PlayerManager.Instance.STR);
                }

                else if (curAnim("comboSlash3"))
                {
                    target.TakeDamage(3 + PlayerManager.Instance.STR);
                }

                else if (curAnim("comboSlash4"))
                {
                    target.TakeDamage(5 + PlayerManager.Instance.STR);
                }
            }

            if (other.gameObject.tag.Equals("Boss"))
            {
                target = other.GetComponent<Enemy>();
                Debug.Log(target.gameObject.name + "을(를) 공격했습니다.");
                if (curAnim("comboSlash1"))
                {
                    Instantiate(hitEffect, hitPos.transform.position, hitPos.transform.rotation);

                    target.TakeDamage(1 + PlayerManager.Instance.STR);
                }
                else if (curAnim("comboSlash2"))
                {
                    Instantiate(hitEffect, hitPos.transform.position, hitPos.transform.rotation);

                    target.TakeDamage(1 + PlayerManager.Instance.STR);
                }

                else if (curAnim("comboSlash3"))
                {
                    Instantiate(hitEffect, hitPos.transform.position, hitPos.transform.rotation);

                    target.TakeDamage(3 + PlayerManager.Instance.STR);
                }

                else if (curAnim("comboSlash4"))
                {
                    Instantiate(hitEffect, target.transform.GetChild(0).transform.position, target.transform.rotation);

                    target.TakeDamage(5 + PlayerManager.Instance.STR);
                }
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
