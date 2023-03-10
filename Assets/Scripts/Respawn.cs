using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject Player;
    public List<GameObject> FoundObjects;
    public GameObject RespawnArea;
    public float shortDis;

    public GameObject RespawnEffect;

    private void Awake()
    {
        Player = GameObject.Find("Player");
    }

    public void RespawnFunc()
    {
        FoundObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("RespawnArea"));
        shortDis = Vector3.Distance(gameObject.transform.position, FoundObjects[0].transform.position); // 첫번째를 기준으로 잡아주기 

        RespawnArea = FoundObjects[0];

        foreach (GameObject found in FoundObjects)
        {
            float Distance = Vector3.Distance(Player.transform.position, found.transform.position);

            if (Distance < shortDis)
            {
                RespawnArea = found;
                shortDis = Distance;
            }
        }

        Player.transform.position = RespawnArea.transform.position;
        PlayerManager.Instance.TakeDamage(PlayerManager.Instance.MAXHP * 20 / 100);
        Instantiate(RespawnEffect, Player.transform.position, Player.transform.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Debug.Log("Trigger 리스폰지역!!");
            RespawnFunc();
        }
    }
}
