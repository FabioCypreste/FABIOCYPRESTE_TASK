using System;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager InventoryManagerInstance { get; private set; }
    public List<InventorySlot> slotsList = new List<InventorySlot>();
    public List<ItemData> allItemsList;
    public int maxSlots = 10;
    public event Action OnInventoryChanged;
    private const string SaveKey = "InventorySaveData";

    void Awake()
    {
        //Avoid duplicates
        if (InventoryManagerInstance != null && InventoryManagerInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        InventoryManagerInstance = this;
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
            foreach(InventorySlot slot in slotsList)
            {
                if (slot.Data == itemToAdd && slot.Quantity < itemToAdd.MaxStackSize)
                {
                    slot.AddQuantity(amount);
                    OnInventoryChanged?.Invoke();
                    return;
                }
            }
        }

        //If don't stack, create a new slot
        if (slotsList.Count < maxSlots)
        {
            slotsList.Add(new InventorySlot(itemToAdd, amount));
            OnInventoryChanged?.Invoke();
        }
    }

    public void ClearInventory()
    {
        slotsList.Clear();
        PlayerPrefs.DeleteKey(SaveKey);
        PlayerPrefs.Save();
        OnInventoryChanged?.Invoke();
    }
    public void RemoveItem(int index)
    {
        if (index >= 0 && index < slotsList.Count)
        {
            slotsList.RemoveAt(index);
            OnInventoryChanged?.Invoke();
        }
    }

    public bool HasItem(ItemData item, int requiredAmount = 1)
    {
        foreach( var slot in slotsList)
        {
            if (slot.Data == item && slot.Quantity >= requiredAmount)
            {
                return true;
            }
        }
        return false;
    }

    //Save system has been got from a previous project that I've made. 
    public void SaveInventory()
    {
        InventorySaveData data = new InventorySaveData();

        foreach (var slot in slotsList)
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

    //Load system has been got from a previous project that I've made. 
    public void LoadInventory()
    {
        if (!PlayerPrefs.HasKey(SaveKey)) return;

        string json = PlayerPrefs.GetString(SaveKey);
        InventorySaveData data = JsonUtility.FromJson<InventorySaveData>(json);

        slotsList.Clear();

        foreach (var savedSlot in data.savedSlots)
        {
            ItemData item = allItemsList.Find(i => i.ID == savedSlot.itemID);
            if (item != null)
            {
                slotsList.Add(new InventorySlot(item, savedSlot.amount));
            }
        }
        OnInventoryChanged?.Invoke();
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