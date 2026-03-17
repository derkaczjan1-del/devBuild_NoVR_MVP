using UnityEngine;

public class HotbarController : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private InventorySlotUI[] hotbarSlots;

    private int selectedIndex = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SelectSlot(0);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SelectSlot(1);
    }

    void SelectSlot(int index)
    {
        selectedIndex = index;
        RefreshHotbar();
    }

    void RefreshHotbar()
    {
        var items = inventory.GetItems();

        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            if (i < items.Count)
                hotbarSlots[i].SetItem(items[i]);
            else
                hotbarSlots[i].Clear();
        }
    }

    public ItemData GetSelectedItem()
    {
        var items = inventory.GetItems();
        if (selectedIndex < items.Count)
            return items[selectedIndex];

        return null;
    }
}