using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public List<ItemData> allItemsList;
    public int maxSlots = 10;

    public event Action OnInventoryChanged;

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
                if (OnInventoryChanged != null) OnInventoryChanged.Invoke();
                return;
            }
        }

        InventorySlot newSlot = new InventorySlot(itemToAdd, 1);
        slots.Add(newSlot);
        Debug.Log($"New slot created for {itemToAdd.ItemName}");
    }

    public void RemoveItem(int index)
    {
        if (slots[index].Quantity <= 0) slots.RemoveAt(index);
        if (slots[index].Quantity > 0) slots[index].RemoveQuantity(1);
        if (OnInventoryChanged != null) OnInventoryChanged.Invoke();
    }
}
