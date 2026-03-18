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
        icon.preserveAspect = true;
    }

    public void Clear()
    {
        if (icon == null) return;

        //wyczyœæ ikonê i ukryj j¹
        icon.sprite = null;
        icon.enabled = false;
    }

    public void SetSelected(bool state)
    {
        if (selectedBorder != null) selectedBorder.SetActive(state);
    }
}