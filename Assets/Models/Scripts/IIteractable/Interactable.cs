using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Key;

public class Interactable: MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText;
    [SerializeField] InteractableType interactableType;

    Outline outline;
    public string message;

    public UnityEvent onInteraction;

    // Start is called before the first frame update
    void Start()
    {
        /*outline = GetComponent<Outline>();
        DisableOutline();*/
    }

    public void Interact(Transform interactorTransform)
    {
        Debug.Log("Interacted with: " + gameObject.name);
        //onInteraction.Invoke();

    }

    /*public void DisableOutline()
    {
        outline.enabled = false;
    }

    public void EnableOutline()
    {
        outline.enabled = true;
    }*/

    public string GetInteractText(Transform interactorTransform)
    {
        return interactText;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public InteractableType GetInteractableType()
    {
        return InteractableType.Pickup;
    }
}
