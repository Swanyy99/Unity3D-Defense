using ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDamage : MonoBehaviour
{
    private PoolableObject pool;

    private void OnEnable()
    {
        pool = GetComponent<PoolableObject>();
        pool.StartCoroutine(pool.DelayToReturn());
    }

    void Update()
    {
        transform.Translate(new Vector3(0f, 0.4f, 0f) * Time.deltaTime, Space.World);
    }

    private void OnDisable()
    {
        
    }



}
