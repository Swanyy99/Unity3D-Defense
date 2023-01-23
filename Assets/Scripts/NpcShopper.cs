using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcShopper : MonoBehaviour
{
    [SerializeField]
    private GameObject ShopUI;

    

    public void OpenShop()
    {
        ShopUI.SetActive(true);
    }

}
