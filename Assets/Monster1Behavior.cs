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

    [SerializeField]
    public GameObject AxeEffect;

    [SerializeField]
    public GameObject AxePos;

    private Rigidbody rigid;

    [SerializeField]
    private Transform TargetTransform;

    [SerializeField]
    private ParticleSystem AxeVFX;

    private float attackTimer;

    private Animator anim;
    private MonsterPushScript mon;

    private Coroutine axeAttack;

    private CharacterController player;

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
                mon = GetComponentInChildren<MonsterPushScript>();
                
                //gameObject.transform.LookAt(TargetTransform.position);
                var direction = (TargetTransform.transform.position - transform.position).normalized;
                // = new Vector3(0f, 0f, 10f);
                var targetRotation = Quaternion.LookRotation(direction);
                //var targetRotation = Quaternion.Euler(target.transform.rotation.x, target.transform.rotation.y, target.transform.rotation.z);
                //if (transform.rotation.x != 0 || transform.rotation.z != 0) transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
                if (curAnim("Attack") == false)
                    rigid.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, 10f));

                if (attackTimer > 2 && attackTimer < 3) anim.ResetTrigger("Attack");

                if (attackTimer > 3)
                {
                    anim.SetTrigger("Attack");
                    //AxeVFX.Play();
                    StartCoroutine(AxeAtttack());
                    attackTimer = 0;
                }
                //this.transform.Translate(0, 0, Time.deltaTime * 2f);
                if (mon.PlayerCollisionOn == false && curAnim("Attack") == false)
                {
                    anim.SetBool("Move", true);
                    this.transform.Translate(0, 0, Time.deltaTime * 3f);
                    return;
                }


                anim.SetBool("Move", false);
                attackTimer += Time.deltaTime;


            }

            if (target == null)
                transform.rotation = Quaternion.Euler(TargetTransform.rotation.x, TargetTransform.rotation.y, TargetTransform.rotation.z);

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

    bool curAnim(string name)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    public IEnumerator AxeAtttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.55f);
            Instantiate(AxeEffect, AxePos.transform.position, AxePos.transform.rotation);
            break;
        }
        //StopCoroutine(doubleSwordWave);
    }



    //private void OnCollisionStay(Collision other)
    //{
    //    if (other.gameObject.tag.Equals("Player"))
    //    {
    //        //if (moveInput.sqrMagnitude > 1f) moveInput.Normalize();
    //        //moveVec = new Vector3(0f, 0f, 1f);
    //        Debug.Log("보스가 민당.");
    //        player = other.gameObject.GetComponent<CharacterController>();
    //        Vector3 fowardVec = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
    //        Vector3 moveInput = Vector3.forward * 2f;
    //        moveVec = fowardVec * moveInput.z;
    //        player.Move(moveVec * Time.deltaTime);
    //    }
    //}
}
