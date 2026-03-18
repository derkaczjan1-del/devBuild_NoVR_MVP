using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType;
    public GameObject worldPrefab;
}

public enum ItemType
{
    Default,
    Key,
    Crowbar,
    Flashlight
}
