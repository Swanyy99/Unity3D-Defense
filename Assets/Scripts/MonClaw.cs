using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonClaw : MonoBehaviour
{
    private GameObject player;

    private Collider col;

    private Enemy me;

    private void Awake()
    {
        col = GetComponent<Collider>();

    }

    void Start()
    {
        me = GetComponentInParent<Enemy>();
        player = GameObject.Find("Player");
        col.enabled = true;
        StartCoroutine(Attack());

    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Debug.Log("클로 맞음");
            
            PlayerManager.Instance.TakeDamage(me.Damage);
        }
    }


    public IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (gameObject == null)
                break;
            col.enabled = false;
            break;
        }
    }
}
