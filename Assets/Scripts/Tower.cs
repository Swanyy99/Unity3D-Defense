using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
public class Tower : MonoBehaviour
{
    [Header("General")]
    [SerializeField]
    private Transform topParts;
    [SerializeField]
    private Transform bottomParts;

    [Header("Spec")]
    private int damage;
    [SerializeField]
    private float range;
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private int cost;

    [SerializeField]
    private GameObject Rocket;

    [SerializeField]
    private Transform LauncherPosition;

    private Enemy target;
    private float lastShootTime = 0f;


    public int Cost { get { return cost; } private set { cost = value; } }
    public int Damage { get { return damage; } private set { damage = value; } }

    //private void Awake()
    //{
    //    rigid = GetComponent<Rigidbody>();
    //}

    private void Update()
    {
        FindTarget();
        Shoot();
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
        Instantiate(Rocket, LauncherPosition.position, LauncherPosition.rotation);
        //target.TakeDamage(damage);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}