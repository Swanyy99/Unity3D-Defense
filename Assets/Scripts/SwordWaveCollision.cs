using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWaveCollision : MonoBehaviour
{
    [SerializeField]
    private GameObject hitEffect;

    private Coroutine destroySwordWaveCollision;



    private Enemy target;
    private Monster1 target2;

    private void Start()
    {
        destroySwordWaveCollision = StartCoroutine(DestroySwordWaveCollision());
    }

    private void Update()
    {
        transform.Translate(0, 0, Time.deltaTime * 13f);
    }


    private IEnumerator DestroySwordWaveCollision()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.7f);
            Destroy(gameObject);
            break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            //Debug.Log("적과 충돌");
            
                target = other.GetComponent<Enemy>();
                target.TakeDamage(PlayerManager.Instance.STR * 5);
                Instantiate(hitEffect, target.transform.GetChild(0).transform.position, target.transform.rotation);

        }

        if (other.gameObject.tag.Equals("Boss"))
        {
            //Debug.Log("보스와 충돌");

            target = other.GetComponent<Enemy>();
            target.TakeDamage(PlayerManager.Instance.STR * 5);
            Instantiate(hitEffect, target.transform.GetChild(0).transform.position, target.transform.rotation);

        }
    }

}
