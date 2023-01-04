using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HpBarScript1 : MonoBehaviour
{

    private Monster1 enemyStatus;
    private Transform cam;

    public Image HpBarImage;
    public TextMeshProUGUI HpText;

    void Start()
    {
        cam = Camera.main.transform;
        enemyStatus = GetComponentInParent<Monster1>();
    }


    void Update()
    {
        HpText.text = enemyStatus.Hp.ToString() + " / " + enemyStatus.MaxHp.ToString();
        HpBarImage.rectTransform.localScale = new Vector3((float)enemyStatus.Hp / (float)enemyStatus.MaxHp, 1f, 1f);

        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
    }
}