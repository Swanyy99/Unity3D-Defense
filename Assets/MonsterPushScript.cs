using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPushScript : MonoBehaviour
{
    private CharacterController player;

    private Animator anim;

    Vector3 moveVec;

    private Monster1Behavior monster;

    public bool PlayerCollisionOn;

    private Coroutine axeAttack;

    private void Awake()
    {
        anim = GetComponentInParent<Animator>();
        monster = GetComponent<Monster1Behavior>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") && anim.GetBool("Move") == true)
        {
            //if (moveInput.sqrMagnitude > 1f) moveInput.Normalize();
            //moveVec = new Vector3(0f, 0f, 1f);
            Debug.Log("보스가 당신을 밀어내고 있습니다.");
            PlayerCollisionOn = true;
            player = other.gameObject.GetComponent<CharacterController>();
            Vector3 fowardVec = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
            Vector3 moveInput = Vector3.forward * 2f;
            moveVec = fowardVec * moveInput.z;
            player.Move(moveVec * Time.deltaTime);
        }
        

    }

    private void OnTriggerExit(Collider other)
    {
        PlayerCollisionOn = false;
    }
}
