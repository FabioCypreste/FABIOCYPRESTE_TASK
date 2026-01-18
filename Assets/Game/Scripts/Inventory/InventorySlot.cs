using System;

public class InventorySlot
{
    public ItemData Data;
    public int Quantity;

    public InventorySlot(ItemData source, int amount)
    {
        Data = source;
        Quantity = amount;
    }
    public void AddQuantity(int amount) => Quantity += amount;
}
