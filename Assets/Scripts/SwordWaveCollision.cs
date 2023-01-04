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
            Debug.Log("���� �浹");
            
                target = other.GetComponent<Enemy>();
                target.TakeDamage(10);
                Instantiate(hitEffect, target.transform.position, target.transform.rotation);

            
                target2 = other.GetComponent<Monster1>();
                target2.TakeDamage(10);
                Instantiate(hitEffect, target2.transform.position, target2.transform.rotation);
        }
    }

}
