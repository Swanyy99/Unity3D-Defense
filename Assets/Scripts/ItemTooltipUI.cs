using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTooltipUI : MonoBehaviour
{
    public Canvas MyCanvas;
    public RectTransform rect;

    Camera cam { get { return MyCanvas.worldCamera; } }

    private void Awake()
    {
        MyCanvas = GetComponentInParent<Canvas>();
        rect = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        this.transform.position = new Vector3(Input.mousePosition.x + 90, Input.mousePosition.y - 75, Input.mousePosition.z);
        gameObject.transform.SetAsLastSibling();
    }
    void Update()
    {
        //var pos = new Vector3(Input.mousePosition.x + 90, Input.mousePosition.y - 75, Input.mousePosition.z);

        //Vector3 output = Vector2.zero;

        //RectTransformUtility.ScreenPointToWorldPointInRectangle(
        //    MyCanvas.GetComponent<RectTransform>(),
        //    pos,
        //    cam,
        //    out output);

        //rect.position = output;
        this.transform.position = new Vector3(Input.mousePosition.x + 90, Input.mousePosition.y - 75, Input.mousePosition.z);
    }
}
