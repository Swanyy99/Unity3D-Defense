using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCheck : MonoBehaviour
{
    private PlayerController player;
    public List<GameObject> FoundObjects;
    public GameObject RespawnArea;
    public GameObject RespawnEffect;
    public float shortDis;



    private void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Respawn"))
        {
            FoundObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("RespawnArea"));
            shortDis = Vector3.Distance(player.gameObject.transform.position, FoundObjects[0].transform.position); // ù��°�� �������� ����ֱ� 

            RespawnArea = FoundObjects[0];

            foreach (GameObject found in FoundObjects)
            {
                float Distance = Vector3.Distance(player.gameObject.transform.position, found.transform.position);

                if (Distance < shortDis)
                {
                    RespawnArea = found;
                    shortDis = Distance;
                }
            }

            player.gameObject.transform.position = RespawnArea.transform.position;
            Debug.Log("������");
            Instantiate(RespawnEffect, player.gameObject.transform.position, player.gameObject.transform.rotation);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        
    }
}
