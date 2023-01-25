using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LogUI : MonoBehaviour, IPointerDownHandler, IDragHandler
{

    Vector3 FirstMousePos;
    Vector3 MousePos;
    Vector3 GAP;
    [SerializeField]
    private TextMeshProUGUI LogText;

    private void Start()
    {
        StartCoroutine(AutoResetLog());
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
        //gameObject.transform.SetAsLastSibling();

    }

    private IEnumerator AutoResetLog()
    {
        while (true)
        {
            yield return new WaitForSeconds(60f);
            LogManager.Instance.logText.text = "<#1E90FF>[알림]</color><#FFFFFF></color> 로그가 초기화되었습니다.\n";
            LogManager.Instance.StartCoroutine(LogManager.Instance.updateScroll());
        }
    }
}
