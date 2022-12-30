using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlace : MonoBehaviour
{
    [SerializeField]
    private Color normal;

    [SerializeField]
    private Color full;

    [SerializeField]
    private Color highlight;

    private MeshRenderer mesh;

    private bool isOver;

    public Tower tower;

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (!isOver)
        {
            if (tower != null)
                mesh.material.color = full;
            else
                mesh.material.color = normal;
        }

        

    }

    private void OnMouseOver()
    {
        if (GameManager.Instance.BuildMode == false)
            return;

        if (Input.GetMouseButton(1))
            return;

        if (EventSystem.current.IsPointerOverGameObject())
        {
            isOver = false;
            if (tower != null)
                mesh.material.color = full;
            if (tower == null)
                mesh.material.color = normal;

        }
        else
        {
            isOver = true;
            mesh.material.color = highlight;
        }
    }

    private void OnMouseExit()
    {
        isOver = false;
        mesh.material.color = normal;
    }

    private void OnMouseUpAsButton()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (GameManager.Instance.BuildMode == false)
            return;

        if (Input.GetMouseButton(1))
            return;

        if (tower == null)
            BuildManager.Instance.Build(this);

        else if (tower != null)
            BuildManager.Instance.Sell(this);
    }

    

}
