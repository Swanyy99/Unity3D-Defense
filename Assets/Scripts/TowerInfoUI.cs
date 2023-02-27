using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        cam = Camera.main.transform;

        Level.text = "Lv. " + tower.Level.ToString();
        Name.text = tower.PrefName.ToString();
        Durability.text = tower.Durability.ToString() + " / " + tower.MaxDurability.ToString();
        Damage.text = tower.Damage.ToString();
        Type.text = tower.Type.ToString();
        Price.text = tower.Cost.ToString() + " G";
        SellPrice.text = tower.SellCost.ToString() + " G";
        UpgradePrice.text = tower.Upgradecost.ToString() + " G";

        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
    }



    private void Update()
    {
        Durability.text = tower.Durability.ToString() + " / " + tower.MaxDurability.ToString();
        DurabilityBar.rectTransform.localScale = new Vector3((float)tower.Durability / (float)tower.MaxDurability, 1f, 1f);
        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
    }

    public void UpdateStat()
    {
        Level.text = "Lv. " + tower.Level.ToString();
        Name.text = tower.PrefName.ToString();
        Durability.text = tower.Durability.ToString() + " / " + tower.MaxDurability.ToString();
        Damage.text = tower.Damage.ToString();
        Type.text = tower.Type.ToString();
        Price.text = tower.Cost.ToString() + " G";
        SellPrice.text = tower.SellCost.ToString() + " G";
        UpgradePrice.text = tower.Upgradecost.ToString() + " G";
    }

    
}
