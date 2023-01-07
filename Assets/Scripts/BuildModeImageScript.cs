using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildModeImageScript : MonoBehaviour
{
    private Image image;

    private byte a = 130;

    private void Awake()
    {
        image = GetComponent<Image>();

    }

    private void OnEnable()
    {
        StartCoroutine(TransparentSetting());
        //image.color = new Color32((byte)255, (byte)255, (byte)255, (byte)3);

    }



    private IEnumerator TransparentSetting()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            byte i = (byte)Random.Range(-40, 40);

            byte z = (byte)(i + a);



            if (z < 100)
            {
                z = 100;
            }

            if (z > 140)
            {
                z = 140;
            }

            image.color = new Color32((byte)255, (byte)255, (byte)255, z);


            if (GameManager.Instance.BuildMode == false)
                break;
        }
    }
}
