using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int maxSlots = 25;
    [SerializeField] private HotbarController hotbar;

    public System.Action onInventoryChanged;

    private List<ItemData> items = new List<ItemData>();

    public bool AddItem(ItemData item)
    {
        //sprawdzamy specjalne itemy (³am, latarka) i umieszczamy je w hotbarze
        if (item.itemType == ItemType.Crowbar)
        {
            hotbar.SetCrowbar(item);
            return true;
        }

        if (item.itemType == ItemType.Flashlight)
        {
            hotbar.SetFlashlight(item);
            return true;
        }

        //reszta normalnych itemów trafia do zwyk³ego ekwipunku
        if (items.Count >= maxSlots) return false;

        items.Add(item);
        onInventoryChanged?.Invoke(); //odœwie¿enie UI ekwipunku

        return true;
    }

    public bool RemoveItem(ItemData item)
    {
        return items.Remove(item);
    }

    public bool ContainsItem(ItemData item)
    {
        return items.Contains(item);
    }

    public List<ItemData> GetItems()
    {
        return items;
    }
}