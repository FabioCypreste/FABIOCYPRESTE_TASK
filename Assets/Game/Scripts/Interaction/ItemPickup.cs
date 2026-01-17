using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] public Item itemPickup;

    public void Interact()
    {
        if (InventoryManager.instance != null)
        {
            InventoryManager.instance.AddItem(itemPickup);
        }
        else
        {
            Debug.Log("Didn't find Inventory Manager");
        }
    }
}
