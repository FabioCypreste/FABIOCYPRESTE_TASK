using UnityEngine;

public class InventorySlot
{
    public ItemData Data;
    public int Quantity;
    public GameObject heldItem;

    public InventorySlot(ItemData source, int amount)
    {
        Data = source;
        Quantity = amount;
    }
    public void AddQuantity(int amount) => Quantity += amount;
}
