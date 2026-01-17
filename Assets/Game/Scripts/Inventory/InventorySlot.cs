using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    [SerializeField] private ItemData item;
    [SerializeField] private int quantity;

    public InventorySlot(ItemData item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

    public ItemData Data => item;
    public int Quantity => quantity;

    public void AddQuantity(int amount)
    {
        quantity += amount;
    }
}