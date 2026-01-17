using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] public ItemData itemData;

    public void Interact()
    {
        if (InventoryManager.InventoryManagerInstance != null)
        {
            InventoryManager.InventoryManagerInstance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
