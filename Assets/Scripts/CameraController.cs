using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float dragSpeed;

    [SerializeField]
    private float zoomSpeed;

    [SerializeField]
    private float padding;

    
    [SerializeField]
    private float mouseSensitivity;

    public Transform follow;

    Vector2 m_Input;

    private void Update()
    {
        Move();
        Zoom();
        Rotate();
    }

    
    private void Move()
    {
        if (GameManager.Instance.BuildMode == false)
            return;

        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        

        Vector2 pos = Input.mousePosition;

        //if (0 <= pos.x && pos.x <= padding)
        //{
        //    transform.Translate(moveSpeed * Vector3.left * DT(), Space.Self);
        //}
        //if (Screen.width - padding <= pos.x && pos.x <= Screen.width)
        //{
        //    transform.Translate(moveSpeed * Vector3.right * DT(), Space.Self);
        //}

        //if (0 <= pos.y && pos.y <= padding)
        //{
        //    transform.Translate(moveSpeed * Vector3.down * DT(), Space.Self);
        //}
        //if (Screen.height - padding <= pos.y && pos.y <= Screen.height)
        //{
        //    transform.Translate(moveSpeed * Vector3.up * DT(), Space.Self);
        //}


        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(moveSpeed * Vector3.right * DT(), Space.Self);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(moveSpeed * Vector3.left * DT(), Space.Self);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(moveSpeed * Vector3.forward * DT(), Space.Self);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(moveSpeed * Vector3.back * DT(), Space.Self);
        }

    }

    private void Zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        transform.Translate(zoomSpeed * scroll * Vector3.forward * DT(), Space.Self);
    }

    private float DT()
    {
        return Time.deltaTime;
    }

    //private void DragMove()
    //{

    //    if (Input.GetMouseButtonDown(0)) clickPoint = Input.mousePosition;
    //    if (Input.GetMouseButtonDown(0)) prevPoint = clickPoint;

    //    //if (prevPoint == clickPoint) DragTimer = 0;

    //    if (Input.GetMouseButton(0))
    //    {
    //        DragTimer += Time.deltaTime;

    //        //isTouched = true;

    //        if (DragTimer < 0.2)
    //        {

    //        Vector3 position = Camera.main.ScreenToViewportPoint((Vector2)Input.mousePosition - clickPoint);

    //        position.z = position.y;
    //        position.y = .0f;

    //        Vector3 move = -position * (Time.deltaTime * dragSpeed);

    //        float y = transform.position.y;

    //        transform.Translate(move);
    //        transform.transform.position = new Vector3(transform.position.x, y, transform.position.z);
    //        }

    //    }

    //    if (Input.GetMouseButtonUp(0))
    //    {
    //        isTouched= false;
    //        DragTimer = 0;
    //    }
    //}



    void Rotate()
    {
        if (Input.GetMouseButton(1))
        {
            m_Input.x = Input.GetAxis("Mouse X");
            m_Input.y = Input.GetAxis("Mouse Y");

            if (m_Input.magnitude != 0)
            {
                Quaternion q = follow.rotation;
                q.eulerAngles = new Vector3(q.eulerAngles.x + m_Input.y * 1f, q.eulerAngles.y + m_Input.x * 1f, q.eulerAngles.z);
                follow.rotation = q;

            }
        }
    }
}