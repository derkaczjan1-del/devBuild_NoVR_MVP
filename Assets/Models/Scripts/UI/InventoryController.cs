using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private InventoryUI inventoryUIManager;
    [SerializeField] private CanvasGroup background;
    [SerializeField] private GameObject backgroundGameObject;

    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryUI.SetActive(isOpen);
        backgroundGameObject.SetActive(isOpen);

        if (isOpen)
        {
            inventoryUIManager.Refresh();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            background.alpha = isOpen ? 1f : 0f;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}