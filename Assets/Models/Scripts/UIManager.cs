using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject containerGameObject;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private TextMeshProUGUI interactText;

    void Update()
    {
        if (playerInteract.GetInteractableObject() != null)
        {
            ShowInteractText(playerInteract.GetInteractableObject());
        }
        else
        {
            HideInteractText();
        }
    }

    public void ShowInteractText(IInteractable interactable)
    {
        containerGameObject.SetActive(true);
        interactText.text = interactable.GetInteractText();
    }

    public void HideInteractText()
    {
        containerGameObject.SetActive(false);
    }

    public void ChangeInteractText(string text)
    {
        interactText.text = text;
    }
}
