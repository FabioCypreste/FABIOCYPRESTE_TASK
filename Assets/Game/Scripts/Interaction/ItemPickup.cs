using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] public ItemData itemData;

    public void Interact()
    {
        if (InventoryManager.instance != null)
        {
            InventoryManager.instance.AddItem(itemData);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Didn't find Inventory Manager");
        }
    }
}
