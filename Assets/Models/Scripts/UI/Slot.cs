using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;

    public void SetItem(ItemData item)
    {
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void Clear()
    {
        icon.enabled = false;
    }
}