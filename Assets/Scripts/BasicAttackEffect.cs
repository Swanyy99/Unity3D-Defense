using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackEffect : MonoBehaviour
{
    private Coroutine destroyBasicAttackHitEffect;
    private AudioSource Audio;
    private void Start()
    {
        Audio = GetComponent<AudioSource>();
        destroyBasicAttackHitEffect = StartCoroutine(DestroyBasicAttackHitEffect());

        Audio.Play();
    }


    private IEnumerator DestroyBasicAttackHitEffect()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
            break;
        }
    }
}
