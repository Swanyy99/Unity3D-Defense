using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;

public class MonsterAxeEffectDestroy : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(AxeVFXgone());
    }

    void Update()
    {
        
    }

    public IEnumerator AxeVFXgone()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
            break;
        }
        //StopCoroutine(doubleSwordWave);
    }
}
