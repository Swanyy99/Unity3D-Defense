using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject PopupLevel;

    public void OnPointerEnter(PointerEventData eventData)
    {
        PopupLevel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PopupLevel.SetActive(false);
    }
}
