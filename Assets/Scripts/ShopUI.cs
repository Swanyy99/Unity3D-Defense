using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShopUI : MonoBehaviour
{
    [Serializable]
    struct shopItemList
    {
        public string Name;
        public GameObject item;
    }

    //[SerializeField]
    //shopItemList[] ShopItemList;

    [SerializeField]
    private List<shopItemList> ShopItemList;

    private void Awake()
    {
        for (int i = 0; i < ShopItemList.Count; i++)
        {
            Item instanceItem = ShopItemList[i].item.GetComponent<Item>();
            instanceItem.AddShop();
        }
    }




}
