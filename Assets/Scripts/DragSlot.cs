using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    static public DragSlot instance;
    public InventoryUnit dragSlot;

    [SerializeField]
    private Image imageItem;

    public void Start()
    {
        instance = this;
    }

    public void DragSetImage(Image itemImage)
    {
        imageItem.sprite = itemImage.sprite;
        SetColor(1);
        gameObject.transform.SetAsLastSibling();
    }

    public void SetColor(float alpha)
    {
        Color color = imageItem.color;
        color.a = alpha;
        imageItem.color = color;
    }
}
