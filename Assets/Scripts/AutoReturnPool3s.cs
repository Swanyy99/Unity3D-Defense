using ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoReturnPool3s : MonoBehaviour
{
    // Start is called before the first frame update

    private PoolableObject pool;

    private void OnEnable()
    {
        pool = GetComponent<PoolableObject>();

        StartCoroutine(AutoReturn());

    }


    IEnumerator AutoReturn()
    {
        yield return new WaitForSeconds(3f);
        pool.Return();
    }
}
