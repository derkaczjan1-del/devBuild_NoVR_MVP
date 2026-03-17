using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int maxSlots = 25;

    public System.Action onInventoryChanged;

    private List<ItemData> items = new List<ItemData>();

    public bool AddItem(ItemData item)
    {
        if (items.Count >= maxSlots) return false;

        items.Add(item);
        //powiadomienie UI o zmianie ekwipunku
        onInventoryChanged?.Invoke();
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