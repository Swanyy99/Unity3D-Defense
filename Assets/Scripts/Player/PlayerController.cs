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
    private int attckCount;

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
    private GameObject SwordWave;

    private Coroutine doubleSwordWave;

    [SerializeField]
    private CinemachineFreeLook playerCam;

    private bool attacked;
    private float attackTimer;


    float maxDistance = 1.4f;

    

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
        Jump();
        Attack();
        Interaction();
    }

    private void Attack()
    {
        if (GameManager.Instance.BuildMode == true)
            return;

        //if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") == true)
        //{
        //    Debug.Log("공격중이애오");
        //    return;
        //}

        //if (anim.GetCurrentAnimatorStateInfo(0).IsName("Jump") == true ||
        //    anim.GetCurrentAnimatorStateInfo(0).IsName("Move") == true)
        //    return;

        if (Input.GetMouseButtonDown(0) && 
            anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") == false &&
            anim.GetCurrentAnimatorStateInfo(0).IsName("Jump") == false &&
            attacked == false)
        {

            attacked = true;
            anim.SetBool("isMoving", false);
            anim.SetBool("isAttacking", true);
            anim.SetTrigger("Attack");
            Instantiate(SwordWave, AttackPos.transform.position, AttackPos.transform.rotation);
            attckCount += 1;

            //doubleSwordWave = StartCoroutine(DoubleSwordWave());
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




        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") == false)
        {

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
        if (controller.isGrounded == false)
            moveY += Physics.gravity.y * Time.deltaTime;

        if (controller.isGrounded == true)
            moveY = 0;



        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") == true)
        return;



        if (Input.GetButtonDown("Jump") && anim.GetBool("isJumping") == false)
        {
            moveY = jumpSpeed;
            anim.SetBool("isJumping", true);
        }

        else if (IsGround() && moveY < 0 && anim.GetCurrentAnimatorStateInfo(0).IsName("Jump") == true)
        {
            Debug.Log("착지 완료");
            anim.SetBool("isJumping", false);
        }

        

        controller.Move(Vector3.up * moveY * Time.deltaTime);
    }

    public void Interaction()
    {
        if (attacked == true)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= 1.2)
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
            yield return new WaitForSeconds(0.2f);
            Instantiate(SwordWave, AttackPos.transform.position, AttackPos.transform.rotation);
            anim.SetBool("isMoving", false);
            break;
        }
        StopCoroutine(doubleSwordWave);
    }

    private bool IsGround()
    {
        if (controller.isGrounded) return true;

        //if (anim.GetBool("isJumping") == false)
        //    return true;

        if (moveY < 0)
        {
            var ray = new Ray(standard.transform.position + Vector3.up * 0.5f, Vector3.down);


            Debug.DrawRay(standard.transform.position + Vector3.up * 0.5f, Vector3.down * maxDistance, Color.red);

            return Physics.Raycast(ray, maxDistance, jumpLayerMask);
        }

        return false;

    }


}
