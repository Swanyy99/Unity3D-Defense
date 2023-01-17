using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    Vector3 FirstMousePos;
    Vector3 GAP;

    [SerializeField]
    private Button CloseButton;

    [SerializeField]
    private GameObject TooltipUI;

    private Image image;


    private void Start()
    {
        CloseButton.onClick.AddListener(Close);
        image = GetComponent<Image>();

    }


    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);

        transform.position = mousePosition + GAP;

    }

    public void OnPointerDown(PointerEventData eventData)
    {

        FirstMousePos = eventData.position;
        GAP = transform.position - FirstMousePos;
        gameObject.transform.SetAsLastSibling();

    }

    public void Close()
    {
        UIManager.Instance.InventoryOn = false;
        gameObject.SetActive(false);
        TooltipUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

   
}
