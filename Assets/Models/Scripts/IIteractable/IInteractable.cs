using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractableType
{
    Pickup,
    Door,
    Hideout

}
public interface IInteractable
{
    

    void Interact(Transform interactorTransform);
    string GetInteractText(Transform interactorTransform);
    Transform GetTransform();

    InteractableType GetInteractableType();
}
