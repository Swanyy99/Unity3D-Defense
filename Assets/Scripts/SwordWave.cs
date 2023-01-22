using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWave : MonoBehaviour
{
    private Coroutine autoDestroyRoutine;

    //public GameObject ColliderObj;

    private void Awake()
    {
        autoDestroyRoutine = StartCoroutine(AutoDestroyRoutine());
        //Instantiate(ColliderObj, transform.position, transform.rotation);

    }

    private IEnumerator AutoDestroyRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.4f);
            Destroy(gameObject);
            break;
        }
    }



}
