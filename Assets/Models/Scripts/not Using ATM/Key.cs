using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData item;
    [SerializeField] InteractableType interactableType;

    private Inventory inventory;
    // Start is called before the first frame update
    public enum KeyType { Gold, Red, Blue }
    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    public ItemData GetKeyType()
    {
        return item;
    }

    public void Interact(Transform interactorTransform)
    {
        if (inventory.AddItem(item))
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Inventory full!");
        }
    }

    public string GetInteractText(Transform interactorTransform)
    {
        return ("Pick up " + item.ToString() + " Key");
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
