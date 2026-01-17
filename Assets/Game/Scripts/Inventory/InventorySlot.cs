using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    [SerializeField] private ItemData item;
    [SerializeField] public int quantity;

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
    public void RemoveQuantity(int amount)
    {
        quantity -= amount;
    }
}