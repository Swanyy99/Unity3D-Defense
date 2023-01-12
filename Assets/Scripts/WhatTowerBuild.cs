using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhatTowerBuild : MonoBehaviour
{
    private Tower tower;

    private Button button;

    private string BTname;

    private void Start()
    {
        button = this.transform.GetComponent<Button>();

        button.onClick.AddListener(SelectThisTower);
    }
        
    public void SelectThisTower()
    {
        BTname = this.gameObject.name;

        if (BTname == "RocketTowerButton")
        {
            Debug.Log("Tower1 선택");
            BuildManager.Instance.selectedTower = BuildManager.Instance.TowerList1;
        }

        else if (BTname == "LauncherTowerButton")
        {
            Debug.Log("Tower2 선택");
            BuildManager.Instance.selectedTower = BuildManager.Instance.TowerList2;
        }

        else if (BTname == "NuclearTowerButton")
        {
            Debug.Log("Tower3 선택");
            BuildManager.Instance.selectedTower = BuildManager.Instance.TowerList3;
        }
    }
}
