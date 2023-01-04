using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Rigidbody rigid;
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
    private bool attacked;
    private bool BasicAttacked;
    private float BasicAttackTimer;
    private float attackTimer;
    private float DashTimer;
    public bool Attacking;
    [SerializeField]
    private GameObject DashTrail;
    public bool isDash;
    float maxDistance = 0.7f;
    public Sword sword;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
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
        Interaction();
        Dash();

    }
    private void Attack()
    {
        if (GameManager.Instance.BuildMode == true)
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
                attacked == false)
            {
                attacked = true;
                anim.SetBool("isMoving", false);
                anim.SetBool("isAttacking", true);
                anim.SetTrigger("Attack");
                StartCoroutine(DoubleSwordWave());
                //doubleSwordWave = StartCoroutine(DoubleSwordWave());
            }
        }
    }
    private void Move()
    {
        if (GameManager.Instance.BuildMode == true)
        {
            playerCam.enabled = false;
            return;
        }
        else if (GameManager.Instance.BuildMode == false)
        {
            playerCam.enabled = true;
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
                transform.forward = Vector3.Lerp(transform.forward, moveVec, 0.8f);
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
            controller.Move(moveVec * moveSpeed * Time.deltaTime);
            if (moveVec.sqrMagnitude != 0)
            {
                transform.forward = Vector3.Lerp(transform.forward, moveVec, 0.8f);
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

        if (GameManager.Instance.BuildMode == true)
            return;

        if (curAnim("Attack") == true)
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
        if (curAnim("Attack"))
            return;
        //if (curAnim("Jump"))
        //    return;
        if (GameManager.Instance.BuildMode == true)
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift))
            anim.SetBool("isDash", true);
        if (anim.GetBool("isDash") == true)
        {
            anim.SetBool("isJumping", false);
            isDash = true;
            moveSpeed = 6f;
            DashTimer += Time.deltaTime;
            if (DashTimer >= 0.4f)
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
    public void Interaction()
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

        if (Input.GetKeyDown(KeyCode.R))
            GameManager.Instance.GameOn = true;
        if (Input.GetKeyDown(KeyCode.F) && GameManager.Instance.BuildMode == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            GameManager.Instance.BuildMode = false;
        }
        else if (Input.GetKeyDown(KeyCode.F) && GameManager.Instance.BuildMode == false)
        {
            Cursor.lockState = CursorLockMode.None;
            GameManager.Instance.BuildMode = true;
        }
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

    private void GravityCheck()
    {
        if (curAnim("Dash"))
        {
            moveY = 0;
            return;
        }

        moveY += Physics.gravity.y * Time.deltaTime;

        if (IsGround() && moveY < 0)
        {
            moveY = -2f;
        }
    }

    bool curAnim(string name)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
}