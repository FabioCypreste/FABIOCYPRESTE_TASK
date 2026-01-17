using System;
using NUnit.Framework.Interfaces;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    [SerializeField] private Item item;
    [SerializeField] private int quantity;

    public InventorySlot(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

    public Item Data => item;
    public int Quantity => quantity;

    public void AddQuantity(int amount)
    {
        quantity += amount;
    }
}