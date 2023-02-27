using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackEffect : MonoBehaviour
{
    private Coroutine destroyBasicAttackHitEffect;
    private AudioSource audioSource;
    private AudioSource PlayerAudioSource;

    private void Start()
    {
        PlayerAudioSource = GetComponentInParent<AudioSource>();
        audioSource = GetComponent<AudioSource>();
        destroyBasicAttackHitEffect = StartCoroutine(DestroyBasicAttackHitEffect());
        if (PlayerAudioSource.isPlaying)
            PlayerAudioSource.Stop();
        audioSource.Play();
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
