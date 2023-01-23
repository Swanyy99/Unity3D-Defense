using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcShopper : MonoBehaviour
{
    [SerializeField]
    private GameObject ShopUI;

    [SerializeField]
    private CinemachineFreeLook playerCam;


    public void OpenShop()
    {
        InventoryManager.Instance.UI_ON = true;
        InventoryManager.Instance.ShowInven();
        ShopUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        playerCam.m_XAxis.m_MaxSpeed = 0;
        playerCam.m_YAxis.m_MaxSpeed = 0;
    }

}
