using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    private readonly Item item;
    private int quantity = 0;

    public InventorySlot(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

    public Item GetItem()
    {
        return this.item;
    }
    public void SetQuantity(int quantityValue)
    {
        this.quantity += quantityValue;
    }
    public int GetQuantity()
    {
        return this.quantity;
    }
}
