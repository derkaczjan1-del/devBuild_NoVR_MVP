using UnityEngine;

public class HotbarController : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private InventorySlotUI[] hotbarSlots;
    [SerializeField] private ItemHolder itemHolder;

    public System.Action onHotbarChanged;

    private int selectedIndex = 0;
    private ItemData crowbar;
    private ItemData flashlight;

    private void Start()
    {
        UpdateHotbar();
        EquipSelectedItem();
    }

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
        UpdateHotbar();

        // Wyposa¿ wybrany przedmiot do rêki gracza
        EquipSelectedItem();
    }

    void EquipSelectedItem()
    {
        ItemData item = GetSelectedItem();

        if (item != null && item.worldPrefab != null)
        {
            itemHolder.EquipItem(item.worldPrefab);
        }
        else
        {
            itemHolder.ClearItem();
        }
    }

    void UpdateHotbar()
    {
        // Slot 0 = ³om
        if (crowbar != null)
            hotbarSlots[0].SetItem(crowbar);
        else
            hotbarSlots[0].Clear();

        // Slot 1 = latarka
        if (flashlight != null)
            hotbarSlots[1].SetItem(flashlight);
        else
            hotbarSlots[1].Clear();

        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            hotbarSlots[i].SetSelected(i == selectedIndex);
        }
    }

    public ItemData GetSelectedItem()
    {
        switch (selectedIndex)
        {
            case 0: return crowbar;
            case 1: return flashlight;
            default: return null;
        }
    }
    public void SetCrowbar(ItemData item)
    {
        crowbar = item;
        UpdateHotbar();
        EquipSelectedItem();
        onHotbarChanged?.Invoke();
    }

    public void SetFlashlight(ItemData item)
    {
        flashlight = item;
        EquipSelectedItem();
        UpdateHotbar();
    }
}