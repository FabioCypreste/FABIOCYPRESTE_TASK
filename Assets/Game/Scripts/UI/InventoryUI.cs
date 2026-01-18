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
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    void Start()
    {
        //Subscribe to inventory changes to refresh the interface
        InventoryManager.InventoryManagerInstance.OnInventoryChanged += UpdateUI;
        UpdateUI();
    }

    public void RefreshUI()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {

        // Clear existing slot objects before rebuilding
        foreach (Transform child in slotsParent)
        {
            Destroy(child.gameObject);
        }

        var slots = InventoryManager.InventoryManagerInstance.slotsList;

        // Instantiate and configure slot UI elements based on manager data
        for (int i = 0; i < slots.Count; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotsParent);
            if (newSlot.TryGetComponent(out InventorySlotUI slotUI))
            {
                slotUI.SetSlot(i, slots[i].Data, slots[i].Quantity);
            }
        }
    }
    // Returns the container transform for drag-and-drop parenting
    public Transform GetSlotsParent()
    {
        return slotsParent;
    }
}