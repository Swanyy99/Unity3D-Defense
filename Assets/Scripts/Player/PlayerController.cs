using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    //private Rigidbody rigid;
    private Animator anim;
    [SerializeField]
    private float moveY;
    [SerializeField]
    private int attackCount;
    [SerializeField]
    private GameObject standard;

    [Header("Basic")]
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private LayerMask jumpLayerMask;

    [Header("Attack")]
    [SerializeField]
    private GameObject AttackPos;
    [SerializeField]
    private GameObject SwordTrail;
    [SerializeField]
    private GameObject SwordWave;
    private Coroutine doubleSwordWave;
    [SerializeField]
    private CinemachineFreeLook playerCam;
    [SerializeField]
    private CinemachineFreeLook BuildCam;

    [Header("InterAction")]
    [SerializeField]
    private bool showInterActionGizmos;
    [SerializeField]
    private float interActionRange;
    [SerializeField, Range(0f, 360f)]
    private float interActionAngle;

    [Header("Etc")]
    private bool attacked;
    private bool BasicAttacked;
    private float BasicAttackTimer;
    private float attackTimer;
    private float DashTimer;
    public bool Attacking;
    [SerializeField]
    private GameObject DashTrail;
    public bool isDash;
    public Sword sword;
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public float respawnDistance = 0.1f;
    [SerializeField]
    private GameObject ShopUI;
    public LayerMask groundMask;
    public LayerMask RespawnMask;


    public Respawn respawn;
    //private float RespawnTimer;

    //private bool CanRespawnVFX = true;

    public List<GameObject> FoundObjects;
    public GameObject RespawnArea;
    public GameObject RespawnEffect;
    public float shortDis;

    //private bool respawnEffectable = true;

    private void Awake()
    {
        //rigid = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        Move();
        Attack();
        GravityCheck();
        Jump();
        Behave();
        InterAction();
        Dash();

        if (Input.GetKeyDown(KeyCode.LeftBracket))
            PlayerManager.Instance.GainHp(30);
        if (Input.GetKeyDown(KeyCode.RightBracket))
            PlayerManager.Instance.GainMp(30);
        if (Input.GetKeyDown(KeyCode.Alpha0))
            BuildManager.Instance.GoldChange(10000);

    }
    private void Attack()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (GameManager.Instance.BuildMode == true)
            return;

        if (ShopUI.activeSelf)
            return;

        if (curAnim("comboSlash1") || curAnim("comboSlash2") ||
            curAnim("comboSlash3") || curAnim("comboSlash4"))
            SwordTrail.SetActive(true);
        else
            SwordTrail.SetActive(false);

        if (Input.GetMouseButtonDown(0))
        {
            if (!curAnim("Attack") && !curAnim("Jump") &&
                !curAnim("comboSlash1") && !curAnim("comboSlash2"))
            {
                Attacking = true;
                BasicAttacked = true;
                BasicAttackTimer = 0;
                anim.SetInteger("BasicAttack", 1);
            }
            if (curAnim("comboSlash1"))
            {
                BasicAttacked = true;
                BasicAttackTimer = 0;
                anim.SetInteger("BasicAttack", 2);
            }
            if (curAnim("comboSlash2")/* && anim.GetInteger("BasicAttack") == 2*/)
            {
                anim.SetInteger("BasicAttack", 3);
            }
        }

        if (BasicAttacked == true)
        {
            BasicAttackTimer += Time.deltaTime;
            if (BasicAttackTimer >= 0.6f)
            {
                SwordTrail.SetActive(false);
                BasicAttacked = false;
                BasicAttackTimer = 0;
                anim.SetInteger("BasicAttack", 0);
            }
        }

        if (curAnim("Idle") || curAnim("Move"))
        {
            if (Input.GetKeyDown(KeyCode.Z) &&
                curAnim("Attack") == false &&
                curAnim("Jump") == false &&
                attacked == false && PlayerManager.Instance.MP >= 40 - PlayerManager.Instance.INT)
            {
                PlayerManager.Instance.UseMana(40 - PlayerManager.Instance.INT);
                attacked = true;
                anim.SetBool("isMoving", false);
                anim.SetBool("isAttacking", true);
                anim.SetTrigger("Attack");
                StartCoroutine(DoubleSwordWave());
            }
        }
    }
    private void Move()
    {
        if (ShopUI.activeSelf)
            return;

        if (GameManager.Instance.BuildMode == true)
        {
            playerCam.enabled = false;
            BuildCam.enabled = true;
            return;
        }

        else if (GameManager.Instance.BuildMode == false)
        {
            playerCam.enabled = true;
            BuildCam.enabled = false;
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) ||
                Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("isMoving", false);
        }

        if (curAnim("comboSlash1") == true ||
            curAnim("comboSlash2") == true ||
            curAnim("comboSlash3") == true ||
            curAnim("comboSlash4") == true)
        {
            Vector3 fowardVec = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;
            Vector3 rightVec = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z).normalized;
            Vector3 moveInput = Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal");
            if (moveInput.sqrMagnitude > 1f) moveInput.Normalize();
            Vector3 moveVec = fowardVec * moveInput.z + rightVec * moveInput.x;
            controller.Move(moveVec * 0.3f * Time.deltaTime);
            if (moveVec.sqrMagnitude != 0)
            {
                transform.forward = Vector3.Lerp(transform.forward, moveVec, 0.5f);
            }
        }

        if (curAnim("Attack") == false &&
            curAnim("comboSlash1") == false &&
            curAnim("comboSlash2") == false &&
            curAnim("comboSlash3") == false &&
            curAnim("comboSlash4") == false
            )
        {
            Attacking = false;

            anim.SetBool("isAttacking", false);

            Vector3 fowardVec = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;
            Vector3 rightVec = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z).normalized;

            Vector3 moveInput = Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal");
            if (moveInput.sqrMagnitude > 1f) moveInput.Normalize();
            Vector3 moveVec = fowardVec * moveInput.z + rightVec * moveInput.x;
            //rigid.velocity = moveVec * moveSpeed;
            controller.Move(moveVec * ( moveSpeed + ( PlayerManager.Instance.DEX * 1 / 20 ) ) * Time.deltaTime);
            if (moveVec.sqrMagnitude != 0)
            {
                transform.forward = Vector3.Lerp(transform.forward, moveVec, 0.7f);
            }


            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ||
                Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                anim.SetBool("isMoving", true);
            }

        }
    }
    private void Jump()
    {
        if (ShopUI.activeSelf)
            return;

        if (GameManager.Instance.BuildMode == true)
            return;

        if (curAnim("Attack") == true)
            return;

        if (curAnim("Dash") == true)
            return;

        Attacking = false;
        if (Input.GetButtonDown("Jump") && IsGround()
            && anim.GetBool("isJumping") == false &&
            curAnim("comboSlash1") == false &&
            curAnim("comboSlash2") == false &&
            curAnim("comboSlash3") == false &&
            curAnim("comboSlash4") == false)
        {
            //rigid.AddForce(Vector3.up * moveY, ForceMode.Impulse);

            moveY = jumpSpeed;
            anim.SetBool("isJumping", true);
        }
        else if (IsGround() && moveY < 0  && curAnim("Jump") == true)
        {
            anim.SetBool("isJumping", false);
        }

        controller.Move(Vector3.up * moveY * Time.deltaTime);

    }
    public void Dash()
    {
        if (ShopUI.activeSelf)
            return;

        if (curAnim("Attack"))
            return;
        
        if (GameManager.Instance.BuildMode == true)
            return;

        //if (curAnim("Dash"))
        //    return;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!curAnim("Dash") && PlayerManager.Instance.MP >= 5 - PlayerManager.Instance.INT)
            {
                PlayerManager.Instance.UseMana(5 - PlayerManager.Instance.INT);
                anim.SetBool("isDash", true);
            }
        }

        if (anim.GetBool("isDash") == true)
        {
            anim.SetInteger("BasicAttack", 0);
            anim.SetBool("isJumping", false);
            isDash = true;
            moveSpeed = 6f + (PlayerManager.Instance.DEX * 1 / 40);
            DashTimer += Time.deltaTime;

            if (DashTimer >= 0.43f)
            {
                anim.SetBool("isDash", false);
                DashTimer = 0;
                moveSpeed = 5f;
                isDash = false;
            }
        }
    }
    public void OnAttackStart()
    {
        sword.enableSwordCollider();
    }
    public void OnAttackEnd()
    {
        sword.disableSwordCollider();
    }
    public void Behave()
    {

        if (attacked == true)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= 1)
            {
                attacked = false;
                attackTimer = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && WaveManager.Instance.SpawnedMonster == 0)
            GameManager.Instance.GameOn = true;

        if (Input.GetKeyDown(KeyCode.F) && GameManager.Instance.BuildMode == true)
        {
            GameManager.Instance.BuildMode = false;
            GameManager.Instance.TooltipOn = false;
            if(InventoryManager.Instance.UI_ON == false)
                Cursor.lockState = CursorLockMode.Locked;
        }
        else if (Input.GetKeyDown(KeyCode.F) && GameManager.Instance.BuildMode == false/* && InventoryManager.Instance.InventoryOn == false*/)
        {
            InventoryManager.Instance.HideUI();
            Cursor.lockState = CursorLockMode.Confined;
            GameManager.Instance.BuildMode = true;
            GameManager.Instance.TooltipOn = true;
        }

        //else if (Input.GetKeyDown(KeyCode.F) && GameManager.Instance.BuildMode == false && InventoryManager.Instance.InventoryOn == true)
        //{
        //    Cursor.lockState = CursorLockMode.Confined;
        //    GameManager.Instance.BuildMode = true;
        //    GameManager.Instance.TooltipOn = true;
        //}
    }

    private void InterAction()
    {
        if (!Input.GetKeyDown(KeyCode.E))
            return;

        

        // 1. 범위내에 있는가
        Collider[] colliders = Physics.OverlapSphere(transform.position, interActionRange);
        for (int i = 0; i < colliders.Length; i++)
        {
            Vector3 dirToTarget = (colliders[i].transform.position - transform.position).normalized;

            // 2. 각도내에 있는가
            if (Vector3.Dot(transform.forward, dirToTarget) < Mathf.Cos(interActionAngle * 0.5f * Mathf.Deg2Rad))
                continue;

            IInteractable target = colliders[i].GetComponent<IInteractable>();
            target?.Interaction(this);
            //if (target != null)
            //{
            //    attacked = false;
            //    anim.SetBool("isMoving", false);
            //}
            
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (showInterActionGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, interActionRange);

            Vector3 rightDir = AngleToDir(transform.eulerAngles.y + interActionAngle * 0.5f);
            Vector3 leftDir = AngleToDir(transform.eulerAngles.y - interActionAngle * 0.5f);
            Debug.DrawRay(transform.position, rightDir * interActionRange, Color.blue);
            Debug.DrawRay(transform.position, leftDir * interActionRange, Color.blue);
        }
    }

    private Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
    }

    private IEnumerator DoubleSwordWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.6f);
            Instantiate(SwordWave, AttackPos.transform.position, AttackPos.transform.rotation);
            anim.SetBool("isMoving", false);
            break;
        }
        //StopCoroutine(doubleSwordWave);
    }
    private bool IsGround()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);


    }

    private bool OnRespawnArea()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, RespawnMask);
    }

    private void GravityCheck()
    {
        if (curAnim("Dash"))
        {
            moveY = 0.1f;
            return;
        }

        moveY += Physics.gravity.y * Time.deltaTime;

        if (IsGround() && moveY < 0)
        {
            moveY = -2f;
        }
    }

    public void RespawnFunc()
    {
        FoundObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("RespawnArea"));
        shortDis = Vector3.Distance(gameObject.transform.position, FoundObjects[0].transform.position); // 첫번째를 기준으로 잡아주기 

        RespawnArea = FoundObjects[0];

        foreach (GameObject found in FoundObjects)
        {
            float Distance = Vector3.Distance(transform.position, found.transform.position);

            if (Distance < shortDis)
            {
                RespawnArea = found;
                shortDis = Distance;
            }
        }

        moveY = 0;
        gameObject.transform.position = RespawnArea.transform.position;

        //GameObject respawnVFX = PoolManager.Instance.Get(RespawnEffect, transform.position, transform.rotation);
        //if (respawnVFX == null)
        //    return;
        
        Instantiate(RespawnEffect, transform.position, transform.rotation);

        //if (respawnEffectable == true)
        //{
        //    respawnEffectable = false;
        //    StartCoroutine(respawnEffectCoroutine());
        //}


    }

    bool curAnim(string name)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }


    //private IEnumerator respawnEffectCoroutine()
    //{
    //    yield return new WaitForSeconds (0.5f);
    //    respawnEffectable = true;
    //}


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag.Equals("Respawn"))
        {
            RespawnFunc();
        }

    }


}