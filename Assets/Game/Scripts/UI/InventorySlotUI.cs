using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amountText;

    private InventorySlot currentSlot;
    public void SetSlot(InventorySlot slot)
    {
        currentSlot = slot;

        if (slot != null && slot.Data != null)
        {
            iconImage.sprite = slot.Data.Icon;
            iconImage.enabled = true;
            amountText.text = slot.Quantity > 1 ? slot.Quantity.ToString() : "";
        }
        else
        {
            iconImage.enabled = false;
            amountText.text = "";
        }
    }
}
