using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public List<ItemData> allItemsList;

    void Awake()
    {
        instance = this;
    }

    public void AddItem(ItemData itemToAdd)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.Data == itemToAdd && slot.Quantity < itemToAdd.MaxStackSize)
            {
                slot.AddQuantity(1);
                Debug.Log($"Item stacked, Total: {slot.Quantity}");
                return;
            }
        }

        InventorySlot newSlot = new InventorySlot(itemToAdd, 1);
        slots.Add(newSlot);
        Debug.Log($"New slot created for {itemToAdd.ItemName}");
    }

    public void RemoveItem(InventorySlot slot)
    {
            slots.Remove(slot);
    }
}
