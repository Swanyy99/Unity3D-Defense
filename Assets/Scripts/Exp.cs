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

    // Start is called before the first frame update
    private void Start()
    {
        me = GetComponentInParent<Enemy>();
        playerStandard = GameObject.Find("Player").transform.GetChild(0).gameObject;
        rigid = GetComponent<Rigidbody>();
        this.transform.parent = null;
    }

    // Update is called once per frame
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
            Destroy(gameObject);
        }
    }
}
