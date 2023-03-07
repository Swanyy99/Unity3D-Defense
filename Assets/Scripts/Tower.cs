using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    [Header("Tower")]
    [SerializeField]
    private string prefName;
    [SerializeField]
    private int level;


    [Header("Spec")]
    [SerializeField]
    private GameObject Rocket;
    [SerializeField]
    public int damage;
    [SerializeField]
    private string type;
    [SerializeField]
    public int durability;
    [SerializeField]
    private int maxdurability;
    [SerializeField]
    private float range;
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private LayerMask targetMask;

    [Header("Price")]
    [SerializeField]
    private int cost;
    [SerializeField]
    private int sellcost;
    [SerializeField]
    private int upgradecost;


    [Header("General")]
    [SerializeField]
    private Transform topParts;
    [SerializeField]
    private Transform bottomParts;
    [SerializeField]
    private Transform EffectPosition;
    [SerializeField]
    private Transform LauncherPosition;
    [SerializeField]
    private Transform LauncherPosition2;
    [SerializeField]
    private Transform LauncherPosition3;
    [SerializeField]
    private Transform LauncherPosition4;
    [SerializeField]
    private Transform LauncherPosition5;
    [SerializeField]
    private GameObject TooltipUI;
    [SerializeField]
    private Button UpgradeButton;
    [SerializeField]
    private Button RepairButton;
    [SerializeField]
    private Button SellButton;

    [Header("Effect")]
    [SerializeField]
    private GameObject HighLevelEffect1;
    [SerializeField]
    private GameObject HighLevelEffect2;
    [SerializeField]
    private GameObject UpgradeEffect;
    [SerializeField]
    private GameObject RepairEffect;
    [SerializeField]
    private GameObject SellEffect;
    [SerializeField]
    private Material Level5Effect;
    [SerializeField]
    private Material Level10Effect;

    [Header("Debug")]
    public Enemy target;

    
    private float lastShootTime = 0f;


    public int Level { get { return level; } private set { level = value; } }
    public int Durability { get { return durability; } private set { durability = value; } }
    public int MaxDurability { get { return maxdurability; } private set { maxdurability = value; } }
    public int Cost { get { return cost; } private set { cost = value; } }
    public int SellCost { get { return sellcost; } private set { sellcost = value; } }
    public int Damage { get { return damage; } set { damage = value; } }
    public int Upgradecost { get { return upgradecost; } private set { upgradecost = value; } }
    public string Type { get { return type; } private set { type = value; } }
    public string PrefName { get { return prefName; } private set { prefName = value; } }


    private void Awake()
    {
        UpgradeButton.onClick.AddListener(Upgrade);
        RepairButton.onClick.AddListener(Repair);
        SellButton.onClick.AddListener(Sell);
        target = null;
    }


    private void Update()
    {
        Shoot();
        TooltipShow();
        TowerDestory();
    }

    private void Shoot()
    {
        if (target != null && !target.isActiveAndEnabled) 
        { 
            target = null; 
            return;
        }

        if (target == null) return;

        if (Time.time < lastShootTime + fireRate) return;


        lastShootTime = Time.time;

        Vector3 NuclearPos = new Vector3(target.transform.position.x, target.transform.position.y + 20, target.transform.position.z);

        if (PrefName == "NuclearTower")
        {
            GameObject temp = Instantiate(Rocket, NuclearPos, target.transform.rotation, this.transform);
        }
        else
        {
            GameObject temp = Instantiate(Rocket, LauncherPosition.position, LauncherPosition.rotation, this.transform);
        }

        if (prefName == "LauncherTower")
        {
            GameObject temp2 = Instantiate(Rocket, LauncherPosition2.position, LauncherPosition2.rotation, this.transform);
            GameObject temp3 = Instantiate(Rocket, LauncherPosition3.position, LauncherPosition3.rotation, this.transform);
            GameObject temp4 = Instantiate(Rocket, LauncherPosition4.position, LauncherPosition4.rotation, this.transform);
            GameObject temp5 = Instantiate(Rocket, LauncherPosition5.position, LauncherPosition5.rotation, this.transform);
        }

        //target.TakeDamage(damage);

    }

    private void TooltipShow()
    {
        if (GameManager.Instance.TooltipOn == false)
            TooltipUI.SetActive(false);
        else TooltipUI.SetActive(true);
    }

    private void TowerDestory()
    {
        if (durability <= 0)
        {
            Instantiate(SellEffect, EffectPosition.position, EffectPosition.rotation);
            StartCoroutine(DelayDestroy());
            //Destroy(gameObject);
        }
    }


    private void Upgrade()
    {
        if (BuildManager.Instance.gold >= Upgradecost)
        {
            BuildManager.Instance.gold -= Upgradecost;
            Upgradecost = Upgradecost + (Upgradecost * 1 / 3);
            cost += (int)(Upgradecost / 10);
            level += 1;

            if (level < 5)
                damage += 1;
            else if (level >= 5 && level < 10)
                damage += 2;
            else
                damage += 3;

            if (prefName == "LauncherTower")
                maxdurability += maxdurability * 1 / 10;
            else
                maxdurability += 10;

            durability = maxdurability;
            sellcost = (int)(cost / 3);
            BuildManager.Instance.GoldUpdate();
            Instantiate(UpgradeEffect, transform.position, transform.rotation);

            if (level >= 5 && level < 10)
            {
                topParts.GetComponent<MeshRenderer>().material = Level5Effect;
                bottomParts.GetComponent<MeshRenderer>().material = Level5Effect;
                HighLevelEffect1.SetActive(true);
                HighLevelEffect2.SetActive(true);
            }

            else if (level >= 10)
            {
                topParts.GetComponent<MeshRenderer>().material = Level10Effect;
                bottomParts.GetComponent<MeshRenderer>().material = Level10Effect;
            }

            gameObject.GetComponentInChildren<TowerInfoUI>().UpdateStat();
        }
    }

    private void Repair()
    {
        if (BuildManager.Instance.gold >= cost && durability != maxdurability)
        {
            BuildManager.Instance.gold -= cost;
            BuildManager.Instance.GoldUpdate();
            durability = maxdurability;
            Instantiate (RepairEffect, EffectPosition.position, EffectPosition.rotation);

            gameObject.GetComponentInChildren<TowerInfoUI>().UpdateStat();
        }
    }

    private void Sell()
    {
        BuildManager.Instance.gold += sellcost;
        BuildManager.Instance.GoldUpdate();
        Instantiate(SellEffect, EffectPosition.position, EffectPosition.rotation);
        Destroy(gameObject);
    }

    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(0.05f);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

        private void OnTriggerEnter(Collider other)
        {
        
        }

    private void OnTriggerStay(Collider other)
    {
        if ((other.gameObject.layer == LayerMask.NameToLayer("Enemy") || other.gameObject.layer == LayerMask.NameToLayer("Boss")) && target == null)
        {
            target = other.gameObject.transform.GetComponent<Enemy>();
        }

        if (target != null && target.isActiveAndEnabled) topParts.LookAt(target.transform.position);
        //if (target != null && !target.isActiveAndEnabled) target = null;
    }

    private void OnTriggerExit(Collider other)
    {
        if (target != null) target = null;
    }

}