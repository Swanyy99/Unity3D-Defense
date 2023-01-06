using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss1Pattern : MonoBehaviour
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

    [SerializeField]
    public GameObject AxeEffect;

    [SerializeField]
    public GameObject AxePos;

    private Rigidbody rigid;

    [SerializeField]
    private Transform TargetTransform;

    private float attackTimer;
    private float stopTimer;

    private Animator anim;


    public CharacterController player;

    private float Distance;

    Vector3 moveVec;

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


    public void PlayAxeAttack()
    {
        StartCoroutine(AxeAtttack());
    }

    public void FindTarget()
    {

        if (!curAnim("Death"))
        {

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
                    attackTimer += Time.deltaTime;

                    Distance = Vector3.Distance(target.transform.position, transform.position);

                    var direction = (TargetTransform.transform.position - transform.position).normalized;
                    direction = new Vector3(((TargetTransform.transform.position - transform.position).normalized).x, 0f, ((TargetTransform.transform.position - transform.position).normalized).z);
                    var targetRotation = Quaternion.LookRotation(direction);

                    if (curAnim("Attack") == false)
                        rigid.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, 10f));


                    if (Distance < 0.9f)
                    {
                        Debug.Log("보스가 당신을 밀어내고 있습니다.");
                        player = target.gameObject.GetComponent<CharacterController>();
                        Vector3 fowardVec = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
                        Vector3 moveInput = Vector3.forward * 5f;
                        moveVec = fowardVec * moveInput.z;
                        player.Move(moveVec * Time.deltaTime);
                    }


                    if (Distance <= 2f)
                    {
                        anim.SetBool("Move", false);
                        stopTimer += Time.deltaTime;
                    }


                    if (curAnim("Attack") == false && Distance > 2f)
                    {
                        anim.SetBool("Move", true);
                        this.transform.Translate(0, 0, Time.deltaTime * 4f);
                        stopTimer = 0;
                        return;
                    }

                    else if (attackTimer > 3 && stopTimer > 0.3f)
                    {
                        attackTimer = 0;
                        stopTimer = 0;
                        StartCoroutine(AxeAtttack());
                    }


                }

                if (target == null)
                    transform.rotation = Quaternion.Euler(TargetTransform.rotation.x, TargetTransform.rotation.y, TargetTransform.rotation.z);

                return;
            }

            target = null;

            anim.SetBool("Move", false);
        }

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

    bool curAnim(string name)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    public IEnumerator AxeAtttack()
    {
        while (true)
        {
            anim.SetTrigger("Attack");
            yield return new WaitForSeconds(0.65f);

            if (curAnim("Death"))
                break;

            Instantiate(AxeEffect, AxePos.transform.position, AxePos.transform.rotation);
            
            break;
        }
    }

}
