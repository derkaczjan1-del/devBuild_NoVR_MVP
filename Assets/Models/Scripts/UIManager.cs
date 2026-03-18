using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject containerGameObject;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeSpeed = 10f;

    private float targetAlpha = 0f;
    private IInteractable lastInteractable;

    private void Start()
    {
        canvasGroup.alpha = 0f;
    }

    void Update()
    {
        IInteractable interactable = playerInteract.GetCurrentInteractable();


        if (interactable != lastInteractable)
        {
            if (interactable != null)
            {
                ShowInteractText(interactable);
            }
            else
            {
                HideInteractText();
            }

            lastInteractable = interactable;
        }
        else if (interactable != null)
        {
            //odœwie¿ tekst nawet jeœli obiekt siê nie zmieni³
            interactText.text = interactable.GetInteractText(playerInteract.transform);
        }
        //fade in/out textu
        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, Time.deltaTime * fadeSpeed);
    }

    public void ShowInteractText(IInteractable interactable)
    {
        //pojawianie siê UI
        targetAlpha = 1f;
        interactText.text = interactable.GetInteractText(playerInteract.transform);
    }

    public void HideInteractText()
    {
        //zanikanie UI
        targetAlpha = 0f;
    }

    public void ChangeInteractText(string text)
    {
        interactText.text = text;
    }
}
