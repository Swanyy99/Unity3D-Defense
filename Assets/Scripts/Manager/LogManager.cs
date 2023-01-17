using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : SingleTon<LogManager>
{
    public GameObject LogUI;

    public TextMeshProUGUI logText;

    public ScrollRect scroll_rect;



    public void Start()
    {

        if (logText != null)
            logText.text += "<#1E90FF>[알림]</color><#FFFFFF> 게임을 시작했습니다.</color>" + "\n";

        StartCoroutine(updateScroll());
    }

    private void Update()
    {
        //if (logText != null)
        ShowLogDetect();

    }

    public void ScrollToBottom()
    {
        scroll_rect.normalizedPosition = new Vector2(0, 0);
    }

    public void ShowLogDetect()
    {
        if (!Input.GetKeyDown(KeyCode.F2))
            return;

        bool val = LogUI.activeSelf ? false : true;
        LogUI.SetActive(val);
        

    }

    public IEnumerator updateScroll()
    {
        yield return new WaitForEndOfFrame();
        scroll_rect.verticalNormalizedPosition = 0f;
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)scroll_rect.transform);
    }

}
