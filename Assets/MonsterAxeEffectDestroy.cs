using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;

public class MonsterAxeEffectDestroy : MonoBehaviour
{
    private Collider col;
    private Enemy me;


    void Start()
    {
        col = GetComponent<Collider>();
        me = GetComponentInParent<Enemy>();
        col.enabled = true;
        StartCoroutine(AxeVFXgone());
        StartCoroutine(Attack());
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Debug.Log("보스 도끼에 맞음");
            PlayerManager.Instance.TakeDamage(me.Damage);
        }
    }

    public IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (gameObject == null)
                break;
            col.enabled = false;
            this.transform.parent = null;
            break;
        }
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
