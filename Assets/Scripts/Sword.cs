using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sword : MonoBehaviour
{
    public GameObject sword;

    public void enableSwordCollider()
    {
        sword.GetComponent<BoxCollider>().enabled = true;
    }

    public void disableSwordCollider()
    {
        sword.GetComponent<BoxCollider>().enabled = false;
    }
}
