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

    private Rocket rocket;

    public int Damage;

    [SerializeField]
    private bool isGlobalAttack;

    private void Awake()
    {
        rocket = gameObject.GetComponentInParent<Rocket>();
        Damage = rocket.Damage;
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();

        if (isGlobalAttack == true)
        {
            col.enabled = true;
        }

        effectGoneRoutine = StartCoroutine(EffectGoneRoutine());
        colliderGoneRoutine = StartCoroutine(ColliderGoneRoutine());
    }

    private void Start()
    {
        transform.parent = null;
    }


    private IEnumerator EffectGoneRoutine()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    private IEnumerator ColliderGoneRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        col.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            Debug.Log("堡开 单固瘤 户具");
            target = other.GetComponent<Enemy>();
            target.TakeDamage(Damage);
        }

        if (other.gameObject.tag.Equals("Boss"))
        {
            Debug.Log("堡开 单固瘤 户具");
            target = other.GetComponent<Enemy>();
            target.TakeDamage(Damage);
        }
    }
}
