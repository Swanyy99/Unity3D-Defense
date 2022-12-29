using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;

    private Animator anim;

    private float moveY;

    [SerializeField]
    private GameObject standard;

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpSpeed;

    float maxDistance = 1.8f;


    private void Awake()
    {
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
    }

    private void Move()
    {
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


        //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ||
        //    Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        //{
        //    anim.SetBool("isMoving", true);
        //}

        //else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) ||
        //         Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        //{
        //    anim.SetBool("isMoving", false);
        //}

    }

    private void Jump()
    {
        // do nothing
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
