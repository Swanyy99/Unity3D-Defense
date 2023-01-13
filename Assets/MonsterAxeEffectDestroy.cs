using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;

public class MonsterAxeEffectDestroy : MonoBehaviour
{
    private Collider col;
    private Enemy me;
    private void Awake()
    {
        col = GetComponent<Collider>();
        me = GetComponentInParent<Enemy>();
    }
    void Start()
    {
        StartCoroutine(AxeVFXgone());
        StartCoroutine(Attack());
        col.enabled = true;
    }

    void Update()
    {
        
    }

    public IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (gameObject == null)
                break;
            col.enabled = false;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Debug.Log("���� ������ ����");
            PlayerManager.Instance.TakeDamage(me.Damage);
        }
    }
}
