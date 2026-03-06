using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private Key.KeyType KeyType;

    KeyHolder keyHolder;

    private void Start()
    {
        keyHolder = FindObjectOfType<KeyHolder>();
    }

    public Key.KeyType GetKeyType()
    {
        return KeyType;
    }
   
    public void OpenDoor()
    {
        gameObject.SetActive(false);
    }

    public void Interact(Transform interactorTransform)
    {
        if (keyHolder.ContainsKey(GetKeyType()))
        {
            OpenDoor();
        }
        else
        {
            Debug.Log("You need a " + KeyType.ToString() + " key to open this door.");
        }
       
    }

    public string GetInteractText()
    {
        return "Open " + KeyType.ToString() + " Door";
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
