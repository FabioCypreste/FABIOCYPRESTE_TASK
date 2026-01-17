
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amountText;
    private int slotIndex;
    private InventorySlot currentSlot;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    public void SetSlot(InventorySlot slot, int index)
    {
        currentSlot = slot;
        slotIndex = index;
        canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();

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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentSlot == null) return;
        originalParent = transform.parent;
        transform.SetParent(transform.root); 
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentSlot == null) return;
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        transform.SetParent(originalParent);
        InventoryManager.InventoryManagerInstance.SaveInventory();
        originalParent.GetComponentInParent<InventoryUI>().UpdateUI();
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventorySlotUI draggedSlot = eventData.pointerDrag.GetComponent<InventorySlotUI>();
        if (draggedSlot != null)
        {
            InventoryManager.InventoryManagerInstance.SwapSlots(draggedSlot.slotIndex, this.slotIndex);
        }
    }
}
