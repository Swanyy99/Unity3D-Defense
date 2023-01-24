using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcHelper : MonoBehaviour
{
    [SerializeField]
    private GameObject conversationUI;

    [SerializeField]
    private GameObject NeedConversationTextUI;

    [SerializeField]
    private GameObject Player;

    private void Update()
    {
        if (conversationUI.activeSelf)
            if(Vector3.Distance(Player.transform.position, gameObject.transform.position) > 1.5f)
                conversationUI.SetActive(false);
    }

    public void OpenConversation()
    {
        conversationUI.SetActive(true);
        GameManager.Instance.HelpConversation = true;
        NeedConversationTextUI.SetActive(false);
    }
}
