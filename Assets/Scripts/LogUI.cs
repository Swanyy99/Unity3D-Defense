using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LogUI : MonoBehaviour, IPointerDownHandler, IDragHandler
{

    Vector3 FirstMousePos;
    Vector3 MousePos;
    Vector3 GAP;

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);

        transform.position = mousePosition + GAP;

    }

    public void OnPointerDown(PointerEventData eventData)
    {

        FirstMousePos = eventData.position;
        GAP = transform.position - FirstMousePos;
        //gameObject.transform.SetAsLastSibling();

    }
}
