using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Hideout : MonoBehaviour, IInteractable
{
    public Transform player, hideLocation;
    string interactText = "Hide!";

    void Teleport()
    {
        player.position = hideLocation.position;
    }

    public void Interact(Transform interactorTransform)
    {
        Teleport();
    }
    public string GetInteractText()
    {
        if (player.GetComponent<PlayerInteract>().IsInHideout())
        {
            return interactText = "Leave hideout";
        }
        return interactText = "Hide!";
    }

    public Transform GetTransform()
    {
        return transform;
    }

    

}
