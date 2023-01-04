using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1 : MonoBehaviour
{

    public int MaxHp;
    public int Hp;


    private void Start()
    {

       
        Hp = MaxHp;

    }

    private void Update()
    {
       
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


}