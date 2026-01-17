using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public ItemData Data;
    public int Quantity;
    public GameObject heldItem;

    public InventorySlot(ItemData source, int amount)
    {
        Data = source;
        Quantity = amount;
    }

    public void SetHeldItem(GameObject item)
    {
        heldItem = item;
        heldItem.transform.position = item.transform.position;
    }
    public void AddQuantity(int amount) => Quantity += amount;
    public void RemoveQuantity(int amount) => Quantity -= amount;

        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
