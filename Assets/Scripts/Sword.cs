using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sword : MonoBehaviour
{
    public GameObject sword;
    private AudioSource audioSource;
    public AudioClip SwingSwordSound;

    private void Awake()
    {
        audioSource = GetComponentInParent<AudioSource>();
    }

    public void enableSwordCollider()
    {
        sword.GetComponent<BoxCollider>().enabled = true;
    }

    public void disableSwordCollider()
    {
        sword.GetComponent<BoxCollider>().enabled = false;
    }

    public void playSwingSwordSound()
    {
        audioSource.clip = SwingSwordSound;
        
        audioSource.Play();
    }
}
