using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData item;
    [SerializeField] InteractableType interactableType;

    private Inventory inventory;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();

    }

    public void Interact(Transform interactorTransform)
    {
        inventory.AddItem(item);
        Destroy(gameObject);
    }

    public string GetInteractText(Transform interactorTransform)
    {
        return "Pick up " + item.itemName;
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