using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class BoomEffect : MonoBehaviour
{
    private Coroutine effectGoneRoutine;
    private Coroutine colliderGoneRoutine;

    private BoxCollider col;

    private Rigidbody rigid;

    private Enemy target;

    [SerializeField]
    private bool isGlobalAttack;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();

        effectGoneRoutine = StartCoroutine(EffectGoneRoutine());
        colliderGoneRoutine = StartCoroutine(ColliderGoneRoutine());

        if (isGlobalAttack == true)
            col.enabled = true;
    }


    private IEnumerator EffectGoneRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
    }

    private IEnumerator ColliderGoneRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            col.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            Debug.Log("堡开 单固瘤 户具");
            target = other.GetComponent<Enemy>();
            target.TakeDamage(1);
        }

        if (other.gameObject.tag.Equals("Boss"))
        {
            Debug.Log("堡开 单固瘤 户具");
            target = other.GetComponent<Enemy>();
            target.TakeDamage(1);
        }
    }
}
