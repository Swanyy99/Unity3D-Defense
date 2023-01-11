using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

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


    private Enemy target;
    private float lastShootTime = 0f;


    public int Level { get { return level; } private set { level = value; } }
    public int Durability { get { return durability; } private set { durability = value; } }
    public int MaxDurability { get { return maxdurability; } private set { maxdurability = value; } }
    public int Cost { get { return cost; } private set { cost = value; } }
    public int SellCost { get { return sellcost; } private set { sellcost = value; } }
    public int Damage { get { return damage; } private set { damage = value; } }
    public int Upgradecost { get { return upgradecost; } private set { upgradecost = value; } }
    public string Type { get { return type; } private set { type = value; } }
    public string PrefName { get { return prefName; } private set { prefName = value; } }


    private void Awake()
    {
        UpgradeButton.onClick.AddListener(Upgrade);
        RepairButton.onClick.AddListener(Repair);
        SellButton.onClick.AddListener(Sell);

    }



    private void Update()
    {
        FindTarget();
        Shoot();
        TooltipShow();
        TowerDestory();
    }

    private void FindTarget()
    {
        target = null;
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        for (int i = 0; i < colliders.Length; i++)
        {
            target = colliders[i].GetComponent<Enemy>();
            if (null != target)
            {

                topParts.LookAt(colliders[i].transform.position);
                //topParts.GetComponent<Rigidbody>().MoveRotation(Quaternion.RotateTowards(topParts.rotation, colliders[i].transform.rotation, 10f));
                break;  
            }
        }
    }

    private void Shoot()
    {
        if (null == target)
            return;
        if (Time.time < lastShootTime + fireRate)
            return;

        lastShootTime = Time.time;

        GameManager.Instance.target = target;
        //durability -= 1;
        GameObject temp = Instantiate(Rocket, LauncherPosition.position, LauncherPosition.rotation);
        temp.transform.parent = this.transform;
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
            Destroy(gameObject);
        }
    }


    private void Upgrade()
    {
        if (BuildManager.Instance.gold >= Upgradecost)
        {
            BuildManager.Instance.gold -= Upgradecost;
            Upgradecost = Upgradecost + (Upgradecost * 1 / 3);
            cost += (int)(Upgradecost / 5);
            level += 1;

            if (level < 5)
                damage += 1;
            else if (level >= 5 && level < 10)
                damage += 2;
            else
                damage += 3;

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
        }
    }

    private void Sell()
    {
        BuildManager.Instance.gold += sellcost;
        BuildManager.Instance.GoldUpdate();
        Instantiate(SellEffect, EffectPosition.position, EffectPosition.rotation);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }


}