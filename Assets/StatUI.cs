using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatUI : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (anim.GetBool("Appear") == true)
                anim.SetBool("Appear", false);
            else
                anim.SetBool("Appear", true);
        }
    }
}
