using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] public ItemData itemData;
    public AudioClip pickupSound;
    public void Interact()
    {
        if (InventoryManager.InventoryManagerInstance != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            InventoryManager.InventoryManagerInstance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
