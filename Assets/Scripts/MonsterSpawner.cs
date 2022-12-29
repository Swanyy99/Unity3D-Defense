using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject Monster;

    [SerializeField]
    private float SpawnCooltime;

    [SerializeField]
    private float RemainCooltime;

    private void Start()
    {
        RemainCooltime = SpawnCooltime;
    }

    private void Update()
    {
        RemainCooltime -= Time.deltaTime;

        if (RemainCooltime <= 0f)
        {
            Instantiate(Monster, this.transform.position, this.transform.rotation);
            RemainCooltime = SpawnCooltime;
        }

    }
}
