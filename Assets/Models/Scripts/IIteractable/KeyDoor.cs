using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData requiredKey;

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

    public string GetInteractText()
    {
        return "Open door";
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
