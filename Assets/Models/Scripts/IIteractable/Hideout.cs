using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Hideout : MonoBehaviour, IInteractable
{

    [SerializeField] InteractableType interactableType;

    public Transform hideLocation;


    public void Interact(Transform interactorTransform)
    {
        interactorTransform.position = hideLocation.position;
    }

    public string GetInteractText(Transform interactorTransform)
    {
        {
            PlayerInteract player = interactorTransform.GetComponent<PlayerInteract>();

            Debug.Log("Hideout.GetInteractText CALLED");
            Debug.Log("Player isInHideout: " + (player != null ? player.IsInHideout().ToString() : "NULL"));

            if (player != null && player.IsInHideout())
            {
                return "Leave hideout";
            }

            return "Hide!";
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public InteractableType GetInteractableType()
    {
        return InteractableType.Hideout;
    }

}
