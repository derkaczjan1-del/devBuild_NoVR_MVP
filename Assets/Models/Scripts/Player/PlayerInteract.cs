using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    FirstPersonController firstPersonController;

    bool inHideout = false;
    Vector3 playerLastPostion;

    [SerializeField] float playerReach = 2f;

    void Start()
    {
            firstPersonController = GetComponent<FirstPersonController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) &&!inHideout)
        {
            IInteractable interactable = GetInteractableObject();
            if (interactable != null)
            {
                if(interactable.ToString().Contains("Hideout") && !inHideout)
                {
                    Debug.Log("Interacting with hideout");
                    inHideout = true;
                    playerLastPostion = this.transform.position;
                    Debug.Log("Player last position: " + playerLastPostion);
                    firstPersonController.SetImmovable();
                    this.gameObject.SetActive(false);
                    interactable.Interact(transform);
                    this.gameObject.transform.Rotate(0, 180, 0);
                    this.gameObject.SetActive(true);
                }
                else interactable.Interact(transform);

            }
        }
        else if(Input.GetKeyDown(KeyCode.E) && inHideout)
        {
            if (playerLastPostion != null)
            {
                Debug.Log("Leaving hideout");
                inHideout = false;
                this.gameObject.SetActive(false);
                gameObject.transform.position = playerLastPostion;
                this.gameObject.SetActive(true);
                firstPersonController.SetMovable();
            }
            else Debug.LogError("Player last position is null, cant't leave hideout!");
        }

    }

    public IInteractable GetInteractableObject()
    {
        List<IInteractable> interactables = new List<IInteractable>();
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, playerReach);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                interactables.Add(interactable);
            }
        }

        IInteractable closestInteractable = null;
        foreach (IInteractable interactable in interactables)
        {
            if (closestInteractable == null)
            {
                closestInteractable = interactable;
            }
            else
            {
                if(Vector3.Distance(transform.position, interactable.GetTransform().position) < Vector3.Distance(transform.position, closestInteractable.GetTransform().position))
                {
                    closestInteractable = interactable;
                }
            }
        }

        return closestInteractable;
    }

    public bool IsInHideout()
    {
        return inHideout;
    }

}
