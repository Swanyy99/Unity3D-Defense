using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1 : MonoBehaviour
{

    public int MaxHp;
    public int Hp;

    private CharacterController player;
    private Vector3 moveVec;
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

    private void OnCollisionStay(Collision collision)
    {
        //if (collision.gameObject.tag.Equals("Player"))
        //{
        //    Debug.Log("¿ÀÈ£");
        //    player = collision.gameObject.GetComponent<CharacterController>();
        //    moveVec = new Vector3(0f, 0f, 1f);
        //    player.Move(moveVec * Time.deltaTime);
        //}
    }


}