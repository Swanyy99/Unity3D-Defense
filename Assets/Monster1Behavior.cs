using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1Behavior : MonoBehaviour
{
    public GameObject target;

    [Header("View Detector")]
    [SerializeField]
    private float viewRadius;
    [SerializeField, Range(0f, 360f)]
    private float viewAngle;
    [SerializeField]
    private LayerMask targetMask;
    [SerializeField]
    private LayerMask obstacleMask;

    private Rigidbody rigid;

    [SerializeField]
    private Transform TargetTransform;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {


        //FindTarget();
    }

    private void LateUpdate()
    {
        FindTarget();
    }

    public void FindTarget()
    {
        if (target == null)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        // 1. 범위내에 있는가
        Collider[] targets = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        for (int i = 0; i < targets.Length; i++)
        {
            Vector3 dirToTarget = (targets[i].transform.position - transform.position).normalized;

            // 2. 각도내에 있는가
            if (Vector3.Dot(transform.forward, dirToTarget) < Mathf.Cos(viewAngle * 0.5f * Mathf.Deg2Rad))
                continue;

            // 3. 중간에 장애물이 있는가
            float distToTarget = Vector3.Distance(transform.position, targets[i].transform.position);
            if (Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                continue;


            Debug.DrawRay(transform.position, dirToTarget * distToTarget, Color.red);
            target = targets[i].gameObject;

            TargetTransform = targets[i].GetComponent<Transform>();

            if (target != null)
            {
                //gameObject.transform.LookAt(TargetTransform.position);
                var direction = (TargetTransform.transform.position - transform.position).normalized;
                this.transform.Translate(0, 0, Time.deltaTime * 2f);
                var targetRotation = Quaternion.LookRotation(direction);
                //var targetRotation = Quaternion.Euler(target.transform.rotation.x, target.transform.rotation.y, target.transform.rotation.z);
                rigid.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, 10f));
                Debug.Log("보는중임");
                //this.transform.Translate(0, 0, Time.deltaTime * 2f);
                anim.SetBool("Move", true);
            }

            
            return;
        }

        target = null;

        anim.SetBool("Move", false);

    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 lookDir = AngleToDir(transform.eulerAngles.y);
        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + viewAngle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - viewAngle * 0.5f);

        Debug.DrawRay(transform.position, lookDir * viewRadius, Color.green);
        Debug.DrawRay(transform.position, rightDir * viewRadius, Color.blue);
        Debug.DrawRay(transform.position, leftDir * viewRadius, Color.blue);
    }

    private Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
    }
}
