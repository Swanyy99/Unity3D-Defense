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
    private float shieldTimer;
    private float attackCoolTimer;

    private Animator anim;


    private CharacterController player;

    private PlayerController playerController;

    private float DistanceGap;

    private bool ShieldOn;

    Vector3 moveVec;

    private Enemy me;

    public AudioClip AxeAttackSound;
    public AudioClip ShieldSound;

    public enum state { Idle, Alert ,Move, Attack, DashAttack, Shield, Die }

    state State;


    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<CharacterController>();
        playerController = player.GetComponent<PlayerController>();

        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        me = GetComponent<Enemy>();
        ShieldEffect.SetActive(false);

        State = state.Idle;
    }

    private void Update()
    {
        switch (State)
        {
            case state.Idle: Idle(); break;
            case state.Alert: Alert(); break;
            case state.Move: Move(); break;
            case state.Attack: Attack(); break;
            case state.DashAttack: DashAttack(); break;
            case state.Shield: Shield(); break;
            //case state.Die: Die(); break;
        }

        PushAway();
    }

    public void PushAway()
    {
        DistanceGap = Vector3.Distance(player.transform.position, transform.position);

        if (DistanceGap < 1f)
        {
            Debug.Log("플레이어 밀어내기");
            Vector3 pushDirection = (player.transform.position - transform.position).normalized;
            player.Move(pushDirection * /*playerController.PlayerMoveSpeed * */5f * Time.deltaTime);
        }
    }

    public void Idle()
    {
        LookAround();

        idleTimer += Time.deltaTime;

        if (target != null)
        {
            //anim.SetBool("Move", true);
            //State = state.Move;
            if (DistanceGap > 3f) Init("Move");
            else Init("Alert");
            return;
        }
        

        if (idleTimer > 3 && ShieldOn == false)
        {
            Init("Shield");
        }
        
    }

    public void Move()
    {
        LookAround();

        if (target != null)
        {
            dashTimer += Time.deltaTime;
            attackTimer += Time.deltaTime;

            transform.Translate(0, 0, Time.deltaTime * moveSpeed);

            if (DistanceGap < 1.5f)
            {
                Init("Alert");
                return;
            }

            if (DistanceGap > 8f)
            {
                Init("Idle");
                return;
            }

            if (attackTimer > 3 && dashTimer > 0.3f)
            {
                Init("DashAttack");
                return;
            }
        }

        if (target == null)
        {
            Init("Idle");
            return;
        }
    }

    public void LookAround()
    {
        Collider[] targets;
        targets = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        for (int i = 0; i < targets.Length; i++)
        {
            Vector3 dirToTarget = (targets[i].transform.position - transform.position).normalized;

            // 각도 감지
            if (Vector3.Dot(transform.forward, dirToTarget) < Mathf.Cos(viewAngle * 0.5f * Mathf.Deg2Rad))
                continue;

            // 거리 감지
            float distToTarget = Vector3.Distance(transform.position, targets[i].transform.position);
            if (Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                continue;

            Debug.DrawRay(transform.position, dirToTarget * distToTarget, Color.red);
            target = targets[i].gameObject;

            //DistanceGap = Vector3.Distance(target.transform.position, transform.position);

            var direction = new Vector3(((target.transform.position - transform.position).normalized).x, 0f, ((target.transform.position - transform.position).normalized).z);
            var targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 6f * Time.deltaTime);

        }

        if (targets.Length == 0)
        {
            target = null;
        }

    }

    public void Alert()
    {
        DieDetect();

        LookAround();

        attackTimer += Time.deltaTime;

        Vector3 fowardVec = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
        Vector3 moveInput = Vector3.forward * 5f;
        moveVec = fowardVec * moveInput.z;

        if (DistanceGap <= 2f)
        {
            rigid.velocity = new Vector3(0f, 0f, 0f);
            stopTimer += Time.deltaTime;
            //dashTimer = 0;
        }

        if (DistanceGap > 2f)
        {
            Init("Move");
        }

        if (attackTimer > 3 && stopTimer > 0.3f)
        {
            attackTimer = 1;
            Init("Attack");
        }

    }

    public void Attack()
    {
        DieDetect();

        attackCoolTimer += Time.deltaTime;

        if (attackCoolTimer >= 2)
        {
            attackCoolTimer = 0;
            Init("Idle");
            //if (target == null) 
            //else if (target != null) State = state.Alert;
        }

    }

    public void DashAttack()
    {
        DieDetect();

        attackCoolTimer += Time.deltaTime;

        if (attackCoolTimer >= 2.3)
        {
            attackTimer = 1.5f;
            attackCoolTimer = 0;
            Init("Idle");
            //if (target == null) State = state.Idle;
            //else if (target != null) State = state.Alert;
        }
    }

    public void Shield()
    {
        DieDetect();
        shieldTimer += Time.deltaTime;
        idleTimer = 0;
        ShieldOn = true;
        me.Damagable = false;

        LookAround();

        if (shieldTimer >= 1.7 && target != null)
        {
            Init("Move");
        }
    }

    public void Init(string type)
    {
        switch (type)
        {
            case "Idle":
                Debug.Log("Idle 진입");
                dashTimer = 0;
                //attackTimer = 0;
                me.Damagable = true;
                ShieldEffect.SetActive(false);
                anim.SetBool("Move", false);
                State = state.Idle;
                break;

            case "Move":
                Debug.Log("Move 진입");
                //attackTimer = 0;
                stopTimer = 0;
                idleTimer = 0;
                shieldTimer = 0;
                ShieldOn = false;
                me.Damagable = true;
                anim.SetBool("Move", true);
                ShieldEffect.SetActive(false);
                State = state.Move;
                break;

            case "Shield":
                Debug.Log("Shield 진입");
                anim.SetTrigger("Shield");
                ShieldEffect.SetActive(true);
                me.RecoverHpBoss();
                anim.SetBool("Move", false);
                State = state.Shield;
                break;

            case "Attack":
                Debug.Log("Attack 진입");
                attackCoolTimer = 0;
                //attackTimer = 0;
                stopTimer = 0;
                StartCoroutine(AxeAtttack());
                State = state.Attack;
                break;

            case "DashAttack":
                Debug.Log("DashAttack 진입");
                attackCoolTimer = 0;
                //attackTimer = 0;
                dashTimer = 0;
                StartCoroutine(DashAxeAtttack());
                State = state.DashAttack;
                break;

            case "Alert":
                Debug.Log("Alert 진입");
                anim.SetBool("Move", false);
                State = state.Alert;
                break;

            default: break;
        }
        
    }

    public void DieDetect()
    {
        if (me.Hp <= 0) State = state.Die;
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
        //attackTimer = 0;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.65f);

        if (curAnim("Death"))
            StopAllCoroutines();

        GameObject temp = Instantiate(AxeEffect, AxePos.transform.position, AxePos.transform.rotation);
        temp.transform.parent = this.transform;
    }

    public IEnumerator DashAxeAtttack()
    {
        dashTimer = 0;
        anim.SetTrigger("DashAttack");
        yield return new WaitForSeconds(1.34f);

        if (curAnim("Death"))
            StopAllCoroutines();

        GameObject temp = Instantiate(DashAttackEffect, DashAttackPos.transform.position, DashAttackPos.transform.rotation);
        temp.transform.parent = this.transform;
    }

}
