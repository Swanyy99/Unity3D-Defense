using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackEffect : MonoBehaviour
{
    private Coroutine destroyBasicAttackHitEffect;

    private void Start()
    {
        destroyBasicAttackHitEffect = StartCoroutine(DestroyBasicAttackHitEffect());
    }


    private IEnumerator DestroyBasicAttackHitEffect()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
            break;
        }
    }
}
