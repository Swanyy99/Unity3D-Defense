using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTooltipUI : MonoBehaviour
{

    private void OnEnable()
    {
        this.transform.position = new Vector3(Input.mousePosition.x + 90, Input.mousePosition.y - 60, Input.mousePosition.z);
        gameObject.transform.SetAsLastSibling();
    }
    void Update()
    {
        this.transform.position = new Vector3(Input.mousePosition.x + 90, Input.mousePosition.y - 60, Input.mousePosition.z);
    }
}
