using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform slotsParent;
    [SerializeField] private GameObject slotPrefab;

    void Start()
    {
        InventoryManager.InventoryManagerInstance.OnInventoryChanged += UpdateUI;
        UpdateUI();
    }

    public void UpdateUI()
    {
        foreach (Transform child in slotsParent) Destroy(child.gameObject);
        var slots = InventoryManager.InventoryManagerInstance.slotsList;

        for (int i = 0; i < slots.Count; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotsParent);
            if (newSlot.TryGetComponent(out InventorySlotUI slotUI))
                slotUI.SetSlot(slots[i], i);
        }
    }
}