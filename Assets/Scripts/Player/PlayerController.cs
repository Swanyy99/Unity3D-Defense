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
    public AudioClip DashSound;
    [SerializeField]
    private GameObject ShopUI;
    public LayerMask groundMask;
    public LayerMask RespawnMask;


    public Respawn respawn;

    public List<GameObject> FoundObjects;
    public GameObject RespawnArea;
    public GameObject RespawnEffect;
    public float shortDis;

    private AudioSource audioSource;

    public float PlayerMoveSpeed;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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
            PlayerMoveSpeed = (0.3f * Time.deltaTime);
            controller.Move(moveVec * PlayerMoveSpeed);

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
            PlayerMoveSpeed = (moveSpeed + (PlayerManager.Instance.DEX * 1 / 30)) * Time.deltaTime;
            controller.Move(moveVec * PlayerMoveSpeed);

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

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!curAnim("Dash") && PlayerManager.Instance.MP >= 5 - PlayerManager.Instance.INT)
            {
                PlayerManager.Instance.UseMana(5 - PlayerManager.Instance.INT);
                anim.SetBool("isDash", true);
                audioSource.clip = DashSound;
                audioSource.Play();
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
    public void OnSwordSwingSoundPlay()
    {
        sword.playSwingSwordSound();
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

    }

    private void InterAction()
    {
        if (!Input.GetKeyDown(KeyCode.E))
            return;


        Collider[] colliders = Physics.OverlapSphere(transform.position, interActionRange);
        for (int i = 0; i < colliders.Length; i++)
        {
            Vector3 dirToTarget = (colliders[i].transform.position - transform.position).normalized;

            if (Vector3.Dot(transform.forward, dirToTarget) < Mathf.Cos(interActionAngle * 0.5f * Mathf.Deg2Rad))
                continue;

            IInteractable target = colliders[i].GetComponent<IInteractable>();
            target?.Interaction(this);

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
    }
    private bool IsGround()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);


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

    public void RespawnFunc(GameObject go)
    {
        moveY = 0;
    }

    bool curAnim(string name)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

}