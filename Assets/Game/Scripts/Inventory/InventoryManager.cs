using System;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public List<InventorySlot> slots = new List<InventorySlot>();
    public List<ItemData> allItemsList;
    public int maxSlots = 10;
    public event Action OnInventoryChanged;
    private const string SaveKey = "InventorySaveData";

    void Awake()
    {
        //Avoid duplicates
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        LoadInventory();
    }

    private void OnDisable()
    {
        SaveInventory();
    }

    void OnApplicationQuit()
    {
        SaveInventory();
    }
    public void AddItem(ItemData itemToAdd, int amount = 1)
    {
        if (itemToAdd.IsStackable)
        {
            foreach(InventorySlot slot in slots)
            {
                if (slot.Data == itemToAdd && slot.Quantity < itemToAdd.MaxStackSize)
                {
                    slot.AddQuantity(1);
                    OnInventoryChanged?.Invoke();
                    return;
                }
            }
        }
        

        if (slots.Count < maxSlots)
        {
            slots.Add(new InventorySlot(itemToAdd, amount));
            OnInventoryChanged?.Invoke();
        }
    }

    public void RemoveItem(int index)
    {
        if (index >= 0 && index < slots.Count)
        {
            slots.RemoveAt(index);
            OnInventoryChanged?.Invoke();
        }
    }

    public bool HasItem(ItemData item, int requiredAmount = 1)
    {
        foreach( var slot in slots)
        {
            if (slot.Data == item && slot.Quantity >= requiredAmount)
            {
                return true;
            }
        }
        return false;
    }

    public void SaveInventory()
    {
        InventorySaveData data = new InventorySaveData();

        foreach (var slot in slots)
        {
            if (slot.Data != null)
            {
                data.savedSlots.Add(new SlotSaveData
                {
                    itemID = slot.Data.ID,
                    amount = slot.Quantity
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

        slots.Clear();

        foreach (var savedSlot in data.savedSlots)
        {
            ItemData item = allItemsList.Find(i => i.ID == savedSlot.itemID);
            if (item != null)
            {
                slots.Add(new InventorySlot(item, savedSlot.amount));
            }
        }
        OnInventoryChanged?.Invoke();
    }
}

[Serializable]
public class InventorySlot
{
    public ItemData Data;
    public int Quantity;

    public InventorySlot(ItemData source, int amount)
    {
        Data = source;
        Quantity = amount;
    }

    public void AddQuantity(int amount) => Quantity += amount;
    public void RemoveQuantity(int amount) => Quantity -= amount;
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