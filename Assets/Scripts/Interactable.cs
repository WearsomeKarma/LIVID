
using System;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    private UnityEvent interaction_Handler;

    [SerializeField]
    private string prompt;
    public String Prompt => prompt;

    internal void Interact()
    {
        interaction_Handler?.Invoke();
    }
}
