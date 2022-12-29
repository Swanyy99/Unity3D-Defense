using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    //private bool isTouched;

    //float DragTimer = 0f;


    //Vector2 clickPoint;
    //Vector2 prevPoint;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }


    private void Update()
    {
        Move();
        Zoom();
    }

    private void Move()
    {
        Vector2 pos = Input.mousePosition;
        //if (isTouched)
        //{

            if (0 <= pos.x && pos.x <= padding)
            {
                transform.Translate(moveSpeed * Vector3.left * DT(), Space.World);
            }
            if (Screen.width - padding <= pos.x && pos.x <= Screen.width)
            {
                transform.Translate(moveSpeed * Vector3.right * DT(), Space.World);
            }

            if (0 <= pos.y && pos.y <= padding)
            {
                transform.Translate(moveSpeed * Vector3.back * DT(), Space.World);
            }
            if (Screen.height - padding <= pos.y && pos.y <= Screen.height)
            {
                transform.Translate(moveSpeed * Vector3.forward * DT(), Space.World);
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
        
}