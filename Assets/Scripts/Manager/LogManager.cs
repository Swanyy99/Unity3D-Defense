using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : SingleTon<LogManager>
{

    public TextMeshProUGUI logText;

    public ScrollRect scroll_rect;



    public void Start()
    {
        //ogText = GameObject.Find("logText").GetComponent<Text>();
        //scroll_rect = GameObject.Find("LogUI").GetComponent<ScrollRect>();

        if (logText != null)
            logText.text += "<#1E90FF>[�˸�]</color><#FFFFFF> ������ �����߽��ϴ�.</color>" + "\n";

        StartCoroutine(updateScroll());
    }

    public void ScrollToBottom()
    {
        scroll_rect.normalizedPosition = new Vector2(0, 0);
    }

    public IEnumerator updateScroll()
    {
        yield return new WaitForEndOfFrame();
        scroll_rect.verticalNormalizedPosition = 0f;
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)scroll_rect.transform);
    }

}
