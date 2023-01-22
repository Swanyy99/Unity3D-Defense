using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpPopupUI : MonoBehaviour
{
    private void OnEnable()
    {
        this.transform.position = new Vector3(Input.mousePosition.x + 5, Input.mousePosition.y - 5, Input.mousePosition.z);
        gameObject.transform.SetAsLastSibling();
    }
    void Update()
    {
        this.transform.position = new Vector3(Input.mousePosition.x + 5, Input.mousePosition.y - 5, Input.mousePosition.z);
    }
}
