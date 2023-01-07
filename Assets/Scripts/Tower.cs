using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private int level;
    [SerializeField]
    private string prefName;

    [Header("General")]
    [SerializeField]
    private Transform topParts;
    [SerializeField]
    private Transform bottomParts;

    [Header("Spec")]
    [SerializeField]
    public int damage;
    [SerializeField]
    private int durability;
    [SerializeField]
    private int maxdurability;
    [SerializeField]
    private float range;
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private int cost;
    [SerializeField]
    private int sellcost;
    [SerializeField]
    private int upgradecost;
    [SerializeField]
    private string type;

    [SerializeField]
    private GameObject Rocket;

    [SerializeField]
    private Transform LauncherPosition;

    [SerializeField]
    private GameObject TooltipUI;

    [SerializeField]
    private Button UpgradeButton;

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
        durability -= 1;
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
            Destroy(gameObject);
    }

    private void Upgrade()
    {
        if (BuildManager.Instance.gold >= Upgradecost)
        {
            BuildManager.Instance.gold -= Upgradecost;
            Upgradecost = Upgradecost + (Upgradecost * 1 / 3);
            level += 1;
            damage += 1;
            maxdurability += 10;
            durability = maxdurability;
            BuildManager.Instance.GoldUpdate();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }


}