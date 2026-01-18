using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    [SerializeField] private Transform slotsParent;
    [SerializeField] private GameObject slotPrefab;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        InventoryManager.InventoryManagerInstance.OnInventoryChanged += UpdateUI;
        UpdateUI();
    }

    public void RefreshUI()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        foreach (Transform child in slotsParent)
        {
            Destroy(child.gameObject);
        }

        var slots = InventoryManager.InventoryManagerInstance.slotsList;

        for (int i = 0; i < slots.Count; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotsParent);
            if (newSlot.TryGetComponent(out InventorySlotUI slotUI))
            {
                slotUI.SetSlot(i, slots[i].Data, slots[i].Quantity);
            }
        }
    }

    public Transform GetSlotsParent()
    {
        return slotsParent;
    }
}