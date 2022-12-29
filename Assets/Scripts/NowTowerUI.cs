using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class NowTowerUI : MonoBehaviour /*IPointerMoveHandler*/
{
    [SerializeField]
    private GameObject FollowingUI;

    [SerializeField]
    private TextMeshProUGUI NowTowerText;

    Vector3 mousePos;
    private void Start()
    {
        BuildManager.Instance.selectedTower = BuildManager.Instance.TowerList1;
    }

    private void Update()
    {
        this.transform.position = new Vector3 (Input.mousePosition.x + 50, Input.mousePosition.y - 15, Input.mousePosition.z);

        if (BuildManager.Instance.selectedTower.name == "RocketTower")
            NowTowerText.text = "Rocket";

        else if (BuildManager.Instance.selectedTower.name == "SuperRocketTower")
            NowTowerText.text = "SuperRocket";

        else
            NowTowerText.text = "None";

    }
}
