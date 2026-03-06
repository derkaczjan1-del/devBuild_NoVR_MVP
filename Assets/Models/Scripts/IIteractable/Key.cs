using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    [SerializeField] private KeyType keyType;

    KeyHolder keyHolder;
    // Start is called before the first frame update
    public enum KeyType { Gold, Red, Blue }
    private void Start()
    {
        keyHolder = FindObjectOfType<KeyHolder>();
    }

    public KeyType GetKeyType()
    {
        return keyType;
    }

    public void Interact(Transform interactorTransform)
    {
        keyHolder.AddKey(GetKeyType());
        Destroy(gameObject);
    }

    public string GetInteractText()
    {
        return "Pick up " + keyType.ToString() + " Key";
    }
    public Transform GetTransform()
    {
        return transform;
    }
}
