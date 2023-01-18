using ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp : MonoBehaviour
{
    private Rigidbody rigid;

    private Enemy me;

    private GameObject playerStandard;

    [SerializeField]
    private GameObject ExpVFX;

    private PoolableObject pool;

    private void OnEnable()
    {
        me = GetComponentInParent<Enemy>();
        rigid = GetComponent<Rigidbody>();
        pool = GetComponent<PoolableObject>();
        this.transform.parent = null;
        playerStandard = GameObject.Find("Player").transform.GetChild(0).gameObject;
    }


    private void Update()
    {
        var direction = (playerStandard.transform.position - transform.position).normalized;
        rigid.velocity = transform.forward * 7f;
        var targetRotation = Quaternion.LookRotation(direction);
        rigid.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, 7f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            PlayerManager.Instance.GainExp(me.Exp);
            Instantiate(ExpVFX, playerStandard.transform.position, transform.rotation);
            pool.Return();
            //Destroy(gameObject);
        }
    }
}
