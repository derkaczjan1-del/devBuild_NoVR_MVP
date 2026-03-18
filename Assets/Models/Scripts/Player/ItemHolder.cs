using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] private Transform holdPoint;

    private GameObject currentItem;

    public void EquipItem(GameObject itemPrefab)
    {
        if (currentItem != null)
            Destroy(currentItem);

        currentItem = Instantiate(itemPrefab, holdPoint);
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.transform.localRotation = Quaternion.identity;
    }

    public void ClearItem()
    {
        if (currentItem != null)
            Destroy(currentItem);
    }
}
