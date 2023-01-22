using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy3s : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(AutoDestroy3sCoroutine());
    }


    public IEnumerator AutoDestroy3sCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
            break;
        }
    }
}
