using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData item;

    private Inventory inventory;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
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

    public string GetInteractText()
    {
        return "Pick up " + item.itemName;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}