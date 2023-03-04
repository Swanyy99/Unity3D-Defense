using ObjectPool;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingDamage : MonoBehaviour
{
    private PoolableObject pool;

    private TextMeshPro damage;

    private void Awake()
    {
        damage = GetComponent<TextMeshPro>();
    }

    private void OnEnable()
    {
        pool = GetComponent<PoolableObject>();
        pool.StartCoroutine(pool.DelayToReturn());
    }

    public void Init(int dmg, bool critical)
    {
        damage.color = critical == true ? Color.red : Color.white;
        damage.fontSize = critical == true ? 2.3f : 2.0f;
        damage.text = dmg.ToString();
    }

    private void Update()
    {
        transform.Translate(new Vector3(0f, 0.4f, 0f) * Time.deltaTime, Space.World);
    }

    private void OnDisable()
    {
        
    }



}
