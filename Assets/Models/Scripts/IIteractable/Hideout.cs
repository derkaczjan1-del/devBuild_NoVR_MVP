using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Hideout : MonoBehaviour, IInteractable
{
    public Transform player, hideLocation;
    public GameObject playerGameObject;

    void Teleport()
    {
        playerGameObject.SetActive(false);
        player.position = hideLocation.position;
        player.Rotate(0,180,0);
        playerGameObject.SetActive(true);
    }
    public void Interact(Transform interactorTransform)
    {
        Teleport();
    }
    public string GetInteractText()
    {
        return "Hide!";
    }

    public Transform GetTransform()
    {
        return transform;
    }

    

}
