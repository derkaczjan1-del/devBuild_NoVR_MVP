using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IInteractable;

public class KeyDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData requiredKey;
    [SerializeField] InteractableType interactableType;

    private Inventory inventory;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    public void Interact(Transform interactorTransform)
    {
        if (inventory.ContainsItem(requiredKey))
        {
            OpenDoor();
        }
        else
        {
            Debug.Log("You need " + requiredKey.itemName);
        }
    }

    void OpenDoor()
    {
        gameObject.SetActive(false);
    }

    public string GetInteractText(Transform interactorTransform)
    {
        return "Open door";
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public InteractableType GetInteractableType()
    {
        return InteractableType.Door;
    }
}
