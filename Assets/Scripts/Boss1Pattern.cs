using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss1Pattern : MonoBehaviour
{
    public GameObject target;

    [Header("Detect Player")]
    [SerializeField]
    private float viewRadius;
    [SerializeField, Range(0f, 360f)]
    private float viewAngle;
    [SerializeField]
    private LayerMask targetMask;
    [SerializeField]
    private LayerMask obstacleMask;


    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    public GameObject AxeEffect;

    [SerializeField]
    private GameObject ShieldEffect;

    [SerializeField]
    public GameObject AxePos;

    [SerializeField]
    public GameObject DashAttackEffect;

    [SerializeField]
    public GameObject DashAttackPos;

    private Rigidbody rigid;

    [SerializeField]
    private Transform TargetTransform;

    private float attackTimer;
    private float stopTimer;
    private float dashTimer;
    private float idleTimer;

    private Animator anim;


    public CharacterController player;

    private float DistanceGap;

    private bool ShieldOn;

    Vector3 moveVec;

    private Enemy me;

    public AudioClip AxeAttackSound;
    public AudioClip ShieldSound;

    enum state { Idle, Alert ,Move, Attack, DashAttack, Shield, Die }

    state State = state.Idle;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        me = GetComponent<Enemy>();
        ShieldEffect.SetActive(false);

    }

    private void Update()
    {
        //switch (State)
        //{
        //    case state.Idle: Idle(); break;
        //    case state.Alert: Alert(); break;
        //    case state.Move: Move(); break;
        //    case state.Attack: Attack(); break;
        //    case state.DashAttack: DashAttack(); break;
        //    case state.Shield: Shield(); break;
        //    case state.Die: Die(); break;
        //}
    }

    private void LateUpdate()
    {
        FindTarget();
    }


    public void PlayAxeAttack()
    {
        StartCoroutine(AxeAtttack());
    }

    //public void Idle()
    //{
    //    idleTimer += Time.deltaTime;

    //    if (idleTimer > 3 && ShieldOn == false)
    //    {
    //        anim.SetTrigger("Shield");
    //        ShieldEffect.SetActive(true);
    //        me.RecoverHpBoss();
    //        State = state.Shield;
    //    }

    //    Collider[] targets;

    //    targets = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
    //    for (int i = 0; i < targets.Length; i++)
    //    {
    //        Vector3 dirToTarget = (targets[i].transform.position - transform.position).normalized;

    //        // 각도 감지
    //        if (Vector3.Dot(transform.forward, dirToTarget) < Mathf.Cos(viewAngle * 0.5f * Mathf.Deg2Rad))
    //            continue;

    //        // 거리 감지
    //        float distToTarget = Vector3.Distance(transform.position, targets[i].transform.position);
    //        if (Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
    //            continue;


    //        Debug.DrawRay(transform.position, dirToTarget * distToTarget, Color.red);
    //        target = targets[i].gameObject;

    //        TargetTransform = targets[i].GetComponent<Transform>();
    //    }

    //    if (target != null)
    //    {
    //        IdleInit();
    //        anim.SetBool("Move", true);
    //        State = state.Move;
    //    }

    //}

    //public void Alert()
    //{
    //    DieDetect();
    //    DistanceGap = Vector3.Distance(target.transform.position, transform.position);
    //    attackTimer += Time.deltaTime;

    //    Debug.Log("보스가 당신을 밀어내고 있습니다.");
    //    player = target.gameObject.GetComponent<CharacterController>();
    //    Vector3 fowardVec = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
    //    Vector3 moveInput = Vector3.forward * 5f;
    //    moveVec = fowardVec * moveInput.z;

    //    if (DistanceGap < 0.9f)
    //    {
    //        player.Move(moveVec * Time.deltaTime);
    //    }

    //    if (DistanceGap <= 2f)
    //    {
    //        rigid.velocity = new Vector3(0f, 0f, 0f);
    //        stopTimer += Time.deltaTime;
    //        dashTimer = 0;
    //    }

    //    else if (DistanceGap > 2f)
    //    {
    //        anim.SetBool("Move", true);
    //        stopTimer = 0;
    //        State = state.Move;
    //    }


    //    if (attackTimer > 3 && stopTimer > 0.3f)
    //    {
    //        attackTimer = 0;
    //        stopTimer = 0;
    //        StartCoroutine(AxeAtttack());
    //        State = state.Attack;
    //    }

    //}

    //public void Move()
    //{
    //    DieDetect();

    //    dashTimer += Time.deltaTime;
    //    attackTimer += Time.deltaTime;

    //    Collider[] targets;

    //    targets = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

    //    for (int i = 0; i < targets.Length; i++)
    //    {
    //        Vector3 dirToTarget = (targets[i].transform.position - transform.position).normalized;

    //        // 각도 감지
    //        if (Vector3.Dot(transform.forward, dirToTarget) < Mathf.Cos(viewAngle * 0.5f * Mathf.Deg2Rad))
    //            continue;

    //        // 거리 감지
    //        float distToTarget = Vector3.Distance(transform.position, targets[i].transform.position);
    //        if (Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
    //            continue;


    //        Debug.DrawRay(transform.position, dirToTarget * distToTarget, Color.red);
    //        target = targets[i].gameObject;

    //        TargetTransform = targets[i].GetComponent<Transform>();

    //        DistanceGap = Vector3.Distance(target.transform.position, transform.position);

            
            
    //        var direction = new Vector3(((target.transform.position - transform.position).normalized).x, 0f, ((target.transform.position - transform.position).normalized).z);
    //        var targetRotation = Quaternion.LookRotation(direction);
    //        rigid.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, 10f));
            
    //    }

        

    //    //this.transform.Translate(0, 0, Time.deltaTime * moveSpeed);

    //    if (DistanceGap < 1.5f)
    //    {
    //        anim.SetBool("Move", false);
    //        State = state.Alert;
    //    }

    //    if (DistanceGap > 8f)
    //    {
    //        anim.SetBool("Move", false);
    //        target = null;
    //        State = state.Idle;
    //    }

    //    if (attackTimer > 3 && dashTimer > 0.3f)
    //    {
    //        attackTimer = 0;
    //        dashTimer = 0;
    //        StartCoroutine(DashAxeAtttack());
    //        State = state.DashAttack;
    //    }
    //}

    //public void Attack()
    //{
    //    DieDetect();


    //    if (!curAnim("Attack"))
    //    {
    //        attackTimer = 0;
    //        State = state.Idle;
    //    }

    //}

    //public void DashAttack()
    //{
    //    DieDetect();


    //    if (!curAnim("DashAttack"))
    //    {
    //        attackTimer = 0;
    //        State = state.Idle;
    //    }

    //}

    //public void Shield()
    //{
    //    DieDetect();

    //    idleTimer = 0;
    //    ShieldOn = true;
    //    me.Damagable = false;

    //    if (!curAnim("Shield"))
    //        State = state.Idle;
    //}

    //public void Die()
    //{
    //    // do nothing;
    //}

    //public void DieDetect()
    //{
    //    if (me.Hp <= 0) State = state.Die;
    //}

    public void IdleInit()
    {
        idleTimer = 0;
        ShieldOn = false;
        me.Damagable = true;
        ShieldEffect.SetActive(false);
    }

    public void FindTarget()
    {

        if (!curAnim("Death"))
        {

            if (curAnim("Idle"))
            {
                idleTimer += Time.deltaTime;

                if (idleTimer > 3 && ShieldOn == false)
                {
                    anim.SetTrigger("Shield");
                    ShieldEffect.SetActive(true);
                    ShieldOn = true;
                    idleTimer = 0;
                    me.RecoverHpBoss();
                    me.Damagable = false;

                }
            }
            //
            else if (curAnim("Idle") == false && curAnim("Shield") == false)
            {
                ShieldOn = false;
                me.Damagable = true;
                ShieldEffect.SetActive(false);
                idleTimer = 0;
            }

            if (curAnim("Shield") == false)
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
                        //rigid.constraints = ~RigidbodyConstraints.FreezeRotationY;
                        //rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                        idleTimer = 0;
                        if (!curAnim("DashAttack"))
                            attackTimer += Time.deltaTime;

                        DistanceGap = Vector3.Distance(target.transform.position, transform.position);

                        //var direction = (target.transform.position - transform.position).normalized;
                        var direction = new Vector3(((target.transform.position - transform.position).normalized).x, 0f, ((target.transform.position - transform.position).normalized).z);
                        var targetRotation = Quaternion.LookRotation(direction);

                        if (curAnim("Attack") == false && curAnim("DashAttack") == false)
                            rigid.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, 10f));


                        if (DistanceGap < 0.9f)
                        {
                            Debug.Log("보스가 당신을 밀어내고 있습니다.");
                            player = target.gameObject.GetComponent<CharacterController>();
                            Vector3 fowardVec = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
                            Vector3 moveInput = Vector3.forward * 5f;
                            moveVec = fowardVec * moveInput.z;
                            player.Move(moveVec * Time.deltaTime);
                        }


                        if (DistanceGap <= 2f)
                        {
                            anim.SetBool("Move", false);
                            rigid.velocity = new Vector3(0f, 0f, 0f);
                            stopTimer += Time.deltaTime;
                            dashTimer = 0;
                        }


                        if (curAnim("Attack") == false && curAnim("DashAttack") == false && DistanceGap > 2f)
                        {
                            anim.SetBool("Move", true);
                            this.transform.Translate(0, 0, Time.deltaTime * moveSpeed);
                            stopTimer = 0;
                            dashTimer += Time.deltaTime;
                            //return;
                        }


                        else if (attackTimer > 3 && stopTimer > 0.3f)
                        {
                            attackTimer = 0;
                            stopTimer = 0;
                            StartCoroutine(AxeAtttack());
                        }

                        if (attackTimer > 3 && dashTimer > 0.3f)
                        {
                            //anim.SetBool("Move", false);
                            attackTimer = 0;
                            dashTimer = 0;
                            StartCoroutine(DashAxeAtttack());
                        }


                    }

                    else
                    {


                        rigid.MoveRotation(Quaternion.Euler(0f, 0f, 0f));
                    }

                    return;
                    //return;
                }

                target = null;

                anim.SetBool("Move", false);
            }
        }

        //if (target == null)
        //{
        //    rigid.MoveRotation(Quaternion.Euler(0f,0f,0f));
        //    rigid.
        //    //rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ ;

        //    //transform.rotation = Quaternion.Euler(TargetTransform.rotation.x, TargetTransform.rotation.y, TargetTransform.rotation.z);

        //}
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

            GameObject temp = Instantiate(AxeEffect, AxePos.transform.position, AxePos.transform.rotation);
            temp.transform.parent = this.transform;
            break;
        }
    }

    public IEnumerator DashAxeAtttack()
    {
        while (true)
        {
            anim.SetTrigger("DashAttack");
            yield return new WaitForSeconds(1.34f);

            if (curAnim("Death"))
                break;

            //anim.applyRootMotion = false;
            GameObject temp = Instantiate(DashAttackEffect, DashAttackPos.transform.position, DashAttackPos.transform.rotation);
            temp.transform.parent = this.transform;
            break;
        }
    }

}
