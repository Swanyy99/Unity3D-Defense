using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfoUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI Level;
    [SerializeField]
    private TextMeshProUGUI Name;
    [SerializeField]
    private TextMeshProUGUI Durability;
    [SerializeField]
    private Image DurabilityBar;
    [SerializeField]
    private TextMeshProUGUI Damage;
    [SerializeField]
    private TextMeshProUGUI Type;
    [SerializeField]
    private TextMeshProUGUI Price;
    [SerializeField]
    private TextMeshProUGUI SellPrice;
    [SerializeField]
    private TextMeshProUGUI UpgradePrice;

    



    private Tower tower;
    private Transform cam;

    private void Awake()
    {
        tower = GetComponentInParent<Tower>();
    }



    private void Start()
    {
        cam = Camera.main.transform;
    }


    private void Update()
    {
        Level.text = "Lv. " + tower.Level.ToString();
        Name.text = tower.PrefName.ToString();
        Durability.text = tower.Durability.ToString() + " / " + tower.MaxDurability.ToString();
        Damage.text = tower.Damage.ToString();
        Type.text = tower.Type.ToString();
        Price.text = tower.Cost.ToString() + " G";
        SellPrice.text = tower.SellCost.ToString() + " G";
        UpgradePrice.text = tower.Upgradecost.ToString() + " G";

        DurabilityBar.rectTransform.localScale = new Vector3((float)tower.Durability / (float)tower.MaxDurability, 1f, 1f);

        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
    }

    
}
