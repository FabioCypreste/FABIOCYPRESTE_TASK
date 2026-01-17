using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager InventoryManagerInstance { get; private set; }

    public int maxSlots = 3;
    public List<ItemData> allItemsList;
    public List<InventorySlot> slotsList = new List<InventorySlot>();

    public event Action OnInventoryChanged;
    private const string SaveKey = "InventorySaveData";

    void Awake()
    {
        if (InventoryManagerInstance != null && InventoryManagerInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        InventoryManagerInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LoadInventory();
    }

    void OnApplicationQuit()
    {
        SaveInventory();
    }

    public void AddItem(ItemData itemToAdd, int amount = 1)
    {
        if (itemToAdd.IsStackable)
        {
            foreach (InventorySlot slot in slotsList)
            {
                if (slot.Data == itemToAdd && slot.Quantity < itemToAdd.MaxStackSize)
                {
                    slot.AddQuantity(amount);
                    OnInventoryChanged?.Invoke();
                    return;
                }
            }
        }

        if (slotsList.Count < maxSlots)
        {
            slotsList.Add(new InventorySlot(itemToAdd, amount));
            OnInventoryChanged?.Invoke();
        }
    }

    public void RemoveItem(int index, int amount)
    {
        for (int i = slotsList.Count - 1; i >= 0; i--)
        {
            if (slotsList[i].Data == item)
            {
                slotsList[i].AddQuantity(-amount);
                if (slotsList[i].Quantity <= 0) slotsList.RemoveAt(i);
                break;
            }
        }
        OnInventoryChanged?.Invoke();
    }

    public void SwapSlots(int indexA, int indexB)
    {
        if (indexA < 0 || indexA >= slotsList.Count || indexB < 0 || indexB >= slotsList.Count) return;
        InventorySlot temp = slotsList[indexA];
        slotsList[indexA] = slotsList[indexB];
        slotsList[indexB] = temp;
        OnInventoryChanged?.Invoke();
    }

    public bool HasItem(ItemData item, int requiredAmount = 1)
    {
        foreach (var slot in slotsList)
        {
            if (slot.Data == item && slot.Quantity >= requiredAmount)
            {
                return true;
            }
        }
        return false;
    }

    public void SwapItems(int indexA, int indexB)
    {
        if (indexA == indexB) return;
        if (indexA < 0 || indexA >= slotsList.Count || indexB < 0 || indexB >= slotsList.Count) return;

        InventorySlot temp = slotsList[indexA];
        slotsList[indexA] = slotsList[indexB];
        slotsList[indexB] = temp;

        OnInventoryChanged?.Invoke();
    }

    public void SaveInventory()
    {
        InventorySaveData data = new InventorySaveData();
        foreach (var slot in slotsList)
        {
            data.savedSlots.Add(new SlotSaveData { itemID = slot.Data.ID, amount = slot.Quantity });
        }
        PlayerPrefs.SetString(SaveKey, JsonUtility.ToJson(data));
        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        if (!PlayerPrefs.HasKey(SaveKey)) return;
        var data = JsonUtility.FromJson<InventorySaveData>(PlayerPrefs.GetString(SaveKey));
        slotsList.Clear();
        foreach (var s in data.savedSlots)
        {
            ItemData item = allItemsList.Find(i => i.ID == s.itemID);
            if (item != null) slotsList.Add(new InventorySlot(item, s.amount));
        }
        OnInventoryChanged?.Invoke();
    }

    public void ClearInventory()
    {
        slotsList.Clear();
        PlayerPrefs.DeleteKey(SaveKey);
    }
}

[Serializable]
public class InventorySaveData
{
    public List<SlotSaveData> savedSlots = new List<SlotSaveData>();
}

[Serializable]
public class SlotSaveData
{
    public string itemID;
    public int amount;
}