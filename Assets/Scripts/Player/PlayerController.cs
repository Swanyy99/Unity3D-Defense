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

    private float moveY;

    [SerializeField]
    private GameObject standard;

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpSpeed;

    [Header("Attack")]
    [SerializeField]
    private GameObject AttackPos;
    [SerializeField]
    private GameObject SwordWave;

    private Coroutine doubleSwordWave;

    [SerializeField]
    private CinemachineFreeLook playerCam;


    //float maxDistance = 1.8f;

    

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
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

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") == true)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Attack");
            //Instantiate(SwordWave, AttackPos.transform.position, AttackPos.transform.rotation);
            doubleSwordWave = StartCoroutine(DoubleSwordWave());
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

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") == true)
            return;

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

        else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) ||
                 Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("isMoving", false);
        }

    }

    private void Jump()
    {
        moveY += Physics.gravity.y * Time.deltaTime;


        //if (Input.GetButtonDown("Jump"))
        //{
        //    moveY = jumpSpeed;
        //    anim.SetBool("isJumping", true);
        //}

        //else if (controller.isGrounded && moveY < 0)
        //{
        //    Debug.Log("ÂøÁö ¿Ï·á");
        //    moveY = 0;
        //    anim.SetBool("isJumping", false);
        //}

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") == true)
            return;

        

        if (Input.GetButtonDown("Jump") && controller.isGrounded == true/* && anim.GetBool("isJumping") == false*/)
        {
            anim.SetTrigger("Jump");
            moveY = jumpSpeed;
            anim.SetBool("isJumping", true);
        }

        else if (moveY <= 0 && controller.isGrounded)
        {
            //Debug.Log("ÂøÁö ¿Ï·á");
            //moveY = 0;
            anim.SetBool("isJumping", false);
        }

        controller.Move(Vector3.up * moveY * Time.deltaTime);
    }

    public void Interaction()
    {
        
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
        StopCoroutine(DoubleSwordWave());
    }

    //private bool IsGround()
    //{
    //    if (controller.isGrounded) return true;

    //    if (anim.GetBool("isJumping") == false)
    //        return true;

    //    var ray = new Ray(standard.transform.position + Vector3.up * 0.5f, Vector3.down);


    //    Debug.DrawRay(standard.transform.position + Vector3.up * 0.5f, Vector3.down * maxDistance, Color.red);

    //    return Physics.Raycast(ray, maxDistance);
    //}


}
