using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionAdaptor : MonoBehaviour, IInteractable
{
    public UnityEvent<PlayerController> OnInterAction;

    private void Start()
    {
        
    }

    public void Interaction(PlayerController player)
    {
        OnInterAction?.Invoke(player);
    }
}
