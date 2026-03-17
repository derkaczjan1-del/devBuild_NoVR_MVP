using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private InventorySlotUI[] slots;

    void Start()
    {
        //aktualizacja UI przy zmianie ekwipunku
        inventory.onInventoryChanged += Refresh;
    }

    public void Refresh()
    {
        var items = inventory.GetItems();

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < items.Count)
                slots[i].SetItem(items[i]);
            else
                slots[i].Clear();
        }
    }
}