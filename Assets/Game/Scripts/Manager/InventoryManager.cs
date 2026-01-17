using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static InventoryManager Instance { get; private set; }

    [Header("Configurações")]
    public int maxSlots = 3;
    private const string SaveKey = "InventorySaveData";

    [Header("Dados")]
    public List<InventorySlot> slotsData = new List<InventorySlot>();
    public List<ItemData> allItemsDatabase;

    [Header("Referências Visuais (UI)")]
    public GameObject[] uiSlots;
    public Image dragIconImage;
    public Camera mainCamera;

    private InventorySlot draggedSlotData;
    private int draggedSlotIndex = -1;

    public event Action OnInventoryChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (dragIconImage != null) dragIconImage.gameObject.SetActive(false);
    }

    void Start()
    {
        // Garante que a lista tem o tamanho certo
        while (slotsData.Count < maxSlots)
        {
            slotsData.Add(new InventorySlot());
        }

        LoadInventory();
        UpdateUI();
    }

    void Update()
    {
        if (draggedSlotData != null && dragIconImage != null)
        {
            dragIconImage.transform.position = Input.mousePosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        GameObject clickedObj = eventData.pointerCurrentRaycast.gameObject;
        int index = GetSlotIndex(clickedObj);

        if (index != -1 && slotsData[index].itemData != null)
        {
            draggedSlotIndex = index;
            draggedSlotData = slotsData[index];

            dragIconImage.sprite = draggedSlotData.itemData.Icon;
            dragIconImage.gameObject.SetActive(true);

            SetSlotVisualAlpha(index, 0.5f);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (draggedSlotData == null) return;

        GameObject targetObj = eventData.pointerCurrentRaycast.gameObject;
        int targetIndex = GetSlotIndex(targetObj);

        if (targetIndex != -1)
        {
            SwapOrStackItems(draggedSlotIndex, targetIndex);
        }
        else if (!IsPointerOverUI(eventData))
        {
            DropItemToWorld(draggedSlotIndex);
        }

        draggedSlotData = null;
        draggedSlotIndex = -1;
        dragIconImage.gameObject.SetActive(false);

        UpdateUI();
        SaveInventory();
    }

    private void SwapOrStackItems(int indexA, int indexB)
    {
        if (indexA == indexB) return;

        InventorySlot slotA = slotsData[indexA];
        InventorySlot slotB = slotsData[indexB];

        // Move para slot vazio
        if (slotB.itemData == null)
        {
            slotB.itemData = slotA.itemData;
            slotB.stackSize = slotA.stackSize;
            slotA.Clear();
        }
        // Empilha se for igual
        else if (slotA.itemData == slotB.itemData && slotB.itemData.IsStackable)
        {
            int spaceLeft = slotB.itemData.MaxStackSize - slotB.stackSize;

            if (spaceLeft >= slotA.stackSize)
            {
                slotB.stackSize += slotA.stackSize;
                slotA.Clear();
            }
            else
            {
                slotB.stackSize = slotB.itemData.MaxStackSize;
                slotA.stackSize -= spaceLeft;
            }
        }
        // Troca (Swap)
        else
        {
            ItemData tempItem = slotB.itemData;
            int tempStack = slotB.stackSize;

            slotB.itemData = slotA.itemData;
            slotB.stackSize = slotA.stackSize;

            slotA.itemData = tempItem;
            slotA.stackSize = tempStack;
        }
    }

    private void DropItemToWorld(int slotIndex)
    {
        InventorySlot slot = slotsData[slotIndex];
        if (slot.itemData != null && slot.itemData.prefab != null)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 10f))
            {
                Instantiate(slot.itemData.prefab, hit.point + Vector3.up * 0.5f, Quaternion.identity);
                slot.Clear();
            }
            else
            {
                Instantiate(slot.itemData.prefab, mainCamera.transform.position + mainCamera.transform.forward * 2f, Quaternion.identity);
                slot.Clear();
            }
        }
    }

    public bool AddItem(ItemData itemToAdd, int amount = 1)
    {
        // Tenta empilhar
        if (itemToAdd.IsStackable)
        {
            foreach (InventorySlot slot in slotsData)
            {
                if (slot.itemData == itemToAdd && slot.stackSize < itemToAdd.MaxStackSize)
                {
                    slot.stackSize += amount;
                    UpdateUI();
                    SaveInventory();
                    return true;
                }
            }
        }

        // Tenta slot vazio
        foreach (InventorySlot slot in slotsData)
        {
            if (slot.itemData == null)
            {
                slot.itemData = itemToAdd;
                slot.stackSize = amount;
                UpdateUI();
                SaveInventory();
                return true;
            }
        }

        return false;
    }

    private void UpdateUI()
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            if (i >= slotsData.Count) break;

            Image icon = uiSlots[i].transform.GetChild(0).GetComponent<Image>();
            Text amountText = uiSlots[i].transform.GetChild(1).GetComponent<Text>();

            SetSlotVisualAlpha(i, 1f);

            if (slotsData[i].itemData != null)
            {
                icon.sprite = slotsData[i].itemData.Icon;
                icon.enabled = true;
                if (amountText) amountText.text = slotsData[i].stackSize > 1 ? slotsData[i].stackSize.ToString() : "";
            }
            else
            {
                icon.enabled = false;
                if (amountText) amountText.text = "";
            }
        }
    }

    private int GetSlotIndex(GameObject obj)
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            if (obj == uiSlots[i] || obj.transform.IsChildOf(uiSlots[i].transform))
            {
                return i;
            }
        }
        return -1;
    }

    private void SetSlotVisualAlpha(int index, float alpha)
    {
        Image icon = uiSlots[index].transform.GetChild(0).GetComponent<Image>();
        var color = icon.color;
        color.a = alpha;
        icon.color = color;
    }

    private bool IsPointerOverUI(PointerEventData eventData)
    {
        return eventData.pointerCurrentRaycast.gameObject != null;
    }

    public void SaveInventory()
    {
        InventorySaveData data = new InventorySaveData();

        for (int i = 0; i < slotsData.Count; i++)
        {
            if (slotsData[i].itemData != null)
            {
                data.savedSlots.Add(new SlotSaveData
                {
                    slotIndex = i,
                    itemID = slotsData[i].itemData.ID,
                    amount = slotsData[i].stackSize
                });
            }
        }

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        if (!PlayerPrefs.HasKey(SaveKey)) return;

        string json = PlayerPrefs.GetString(SaveKey);
        InventorySaveData data = JsonUtility.FromJson<InventorySaveData>(json);

        foreach (var slot in slotsData) slot.Clear();

        foreach (var savedSlot in data.savedSlots)
        {
            if (savedSlot.slotIndex < slotsData.Count)
            {
                ItemData item = allItemsDatabase.Find(i => i.ID == savedSlot.itemID);
                if (item != null)
                {
                    slotsData[savedSlot.slotIndex].itemData = item;
                    slotsData[savedSlot.slotIndex].stackSize = savedSlot.amount;
                }
            }
        }
        UpdateUI();
    }
}

// Classes de Save Data (mantidas aqui para facilitar, mas podes separar se quiseres)
[Serializable]
public class InventorySaveData
{
    public List<SlotSaveData> savedSlots = new List<SlotSaveData>();
}

[Serializable]
public class SlotSaveData
{
    public int slotIndex;
    public string itemID;
    public int amount;
}