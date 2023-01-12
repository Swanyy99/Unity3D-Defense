using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OakPattern : MonoBehaviour
{
    [SerializeField]
    private GameObject oakFinger1;
    [SerializeField]
    private GameObject oakFinger2;
    [SerializeField]
    private GameObject oakFinger3;
    [SerializeField]
    private GameObject oakFinger4;
    [SerializeField]
    private GameObject oakFinger5;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (curAnim("Attack"))
        {
            oakFinger1.SetActive(true);
            oakFinger2.SetActive(true);
            oakFinger3.SetActive(true);
            oakFinger4.SetActive(true);
            oakFinger5.SetActive(true);
        }
        else
        {
            oakFinger1.SetActive(false);
            oakFinger2.SetActive(false);
            oakFinger3.SetActive(false);
            oakFinger4.SetActive(false);
            oakFinger5.SetActive(false);
        }
    }


    bool curAnim(string name)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
}
