using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public ItemData itemData;
    [SerializeField] Image iconImage;

    void Update()
    {
        iconImage.sprite = itemData.Icon;
    }
}
