using ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp : BezierCurve
{
    private Enemy me;

    private GameObject playerStandard;

    [SerializeField]
    private GameObject ExpVFX;

    private PoolableObject pool;

    private GameObject MonGate;

    private void Awake()
    {
        MonGate = GameObject.Find("MonsterGate");
    }

    private void OnEnable()
    {
        startObj = null;
        me = GetComponentInParent<Enemy>();
        pool = GetComponent<PoolableObject>();
        this.transform.parent = null;
        playerStandard = GameObject.Find("Player").transform.GetChild(0).gameObject;
        routeAmount = 0;
        traveler = gameObject;
        targetObj = playerStandard;
        if (me != null) startObj = me.gameObject;
        if (me != null) startVec = startObj.transform.position;
    }

    public override void Update()
    {
        base.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            PlayerManager.Instance.GainExp(me.Exp);
            Instantiate(ExpVFX, playerStandard.transform.position, transform.rotation);
            pool.Return();
        }
    }

    private void OnDisable()
    {
        transform.position = MonGate.transform.position;
    }
}
