using UnityEngine;

public class InteractableHighlight : MonoBehaviour
{
    private Outline outline;

    void Awake()
    {
        outline = GetComponentInChildren<Outline>();
        if (outline != null)
        {
            //wy³¹cza podœwietlenie na start
            outline.enabled = false; 
        }
    }

    public void SetHighlight(bool state)
    {
        if (outline != null)
        {
            outline.enabled = state;
        }
    }
}