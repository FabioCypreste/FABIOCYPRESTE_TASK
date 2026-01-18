using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Image iconImage;
    public TextMeshProUGUI amountText;
    public int slotIndex;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void SetSlot(int index, ItemData data, int quantity)
    {
        slotIndex = index;
        if (data != null)
        {
            iconImage.sprite = data.Icon;
            iconImage.enabled = true;
            amountText.text = quantity > 1 ? quantity.ToString() : "";
        }
        else
        {
            iconImage.enabled = false;
            amountText.text = "";
        }
    }

    //Drag, drop and swap methods
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!iconImage.enabled) return;

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!iconImage.enabled) return;
        transform.position = eventData.position; //The item follows the mouse position
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        //Alocates the item in the slow below
        transform.SetParent(InventoryUI.Instance.GetSlotsParent());
        InventoryUI.Instance.UpdateUI();
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventorySlotUI itemA = eventData.pointerDrag.GetComponent<InventorySlotUI>();

        //If there's an item in the slot below, reorder him
        if (itemA != null && itemA != this)
        {
            InventoryManager.InventoryManagerInstance.SwapItems(itemA.slotIndex, this.slotIndex);

            InventoryUI.Instance.UpdateUI();
        }
    }
}