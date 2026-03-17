using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private GameObject selectedBorder;

    public void SetItem(ItemData item)
    {
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void Clear()
    {
        icon.enabled = false;
    }

    public void SetSelected(bool state)
    {
        selectedBorder.SetActive(state);
    }
}