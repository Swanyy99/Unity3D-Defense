using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private int curWayIndex = 0;

    public int MaxHp;
    public int Hp;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {

        SetNextPoint();

        if (WaveManager.Instance.Wave < 10)
        {
            //MaxHp = WaveManager.Instance.Wave + 2;
            MaxHp = WaveManager.Instance.Wave + 99;
            agent.speed = 5;
        }

        if (WaveManager.Instance.Wave >= 10)
        {
            MaxHp = WaveManager.Instance.Wave * 2;
            agent.speed = 7;
        }

        Hp = MaxHp;

    }

    private void Update()
    {
        if (IsArrive())
        {
            if (curWayIndex == WaveManager.Instance.WayPoints.Count - 1)
                OnArriveEndPoint();

            else
                SetNextPoint();
        }
    }

    private bool IsArrive()
    {
        if ((WaveManager.Instance.WayPoints[curWayIndex].position - transform.position).sqrMagnitude < 0.1f)
            return true;
        else
            return false;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("WayPoint"))
    //    {
    //        if (curWayIndex >= WaveManager.Instance.WayPoints.Count - 1)
    //            OnArriveEndPoint();
    //        else
    //            SetNextPoint();
    //    }
    //}

    private void OnArriveEndPoint()
    {
        Debug.Log("À¸¾Ç");
        WaveManager.Instance.TakeDamage(1);
        Destroy(gameObject);
    }

    private void SetNextPoint()
    {
        curWayIndex++;
        agent.destination = WaveManager.Instance.WayPoints[curWayIndex].position;
    }

    public void TakeDamage(int damage)
    {
        Hp -= damage;

        if (Hp <= 0)
        {
            BuildManager.Instance.GainEnergy(1);
            WaveManager.Instance.WaveMonsterDeath += 1;
            Destroy(gameObject);

        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag.Equals("Attack"))
    //    {
    //        TakeDamage(1);
    //    }

    //}
}