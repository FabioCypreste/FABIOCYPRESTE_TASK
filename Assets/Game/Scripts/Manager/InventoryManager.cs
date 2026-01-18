using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager InventoryManagerInstance { get; private set; }
    public List<InventorySlot> slotsList = new List<InventorySlot>();
    public List<ItemData> allItemsList;
    public int maxSlots = 3;
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
        DontDestroyOnLoad(gameObject);
        InitializeInventory();
    }

    void Start()
    {
        LoadInventory();
    }

    private void OnDisable() => SaveInventory();

    void OnApplicationQuit() => SaveInventory();

    private void InitializeInventory()
    {
        if (slotsList.Count == 0)
        {
            for (int i = 0; i < maxSlots; i++) slotsList.Add(new InventorySlot(null, 0));
        }
    }

    public void AddItem(ItemData itemToAdd, int amount = 1)
    {   
        //Try to stack in existing slots
        if (itemToAdd.IsStackable)
        {
            foreach (var slot in slotsList)
            {
                if (slot.Data != null && slot.Data.ID == itemToAdd.ID)
                {
                    slot.AddQuantity(amount);
                    OnInventoryChanged?.Invoke();
                    SaveInventory();
                    return;
                }
            }
        }

        //Fill first available empty slot
        for (int i = 0; i < slotsList.Count; i++)
        {
            if (slotsList[i].Data == null)
            {
                slotsList[i] = new InventorySlot(itemToAdd, amount);
                OnInventoryChanged?.Invoke();
                SaveInventory();
                return;
            }
        }
    }

    public void SwapItems(int indexA, int indexB)
    {
        if (indexA >= 0 && indexA < slotsList.Count && indexB >= 0 && indexB < slotsList.Count)
        {
            InventorySlot temp = slotsList[indexA];
            slotsList[indexA] = slotsList[indexB];
            slotsList[indexB] = temp;
            OnInventoryChanged?.Invoke();
            SaveInventory();
        }
    }

    //Callable from "New game"
    public void ClearInventory()
    {
        //Reset all slots to empty state and wipe persistence
        for (int i = 0; i < slotsList.Count; i++) slotsList[i] = new InventorySlot(null, 0);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        OnInventoryChanged?.Invoke();
    }
    public bool HasItem(ItemData item, int requiredAmount = 1)
    {
        foreach (var slot in slotsList)
        {
            if (slot.Data == item && slot.Quantity >= requiredAmount) return true;
        }
        return false;
    }

    public void RemoveItem(ItemData itemToRemove, int amount)
    {
        for (int i = 0; i < slotsList.Count; i++)
        {
            if (slotsList[i].Data == itemToRemove)
            {
                slotsList[i].Quantity -= amount;
                if (slotsList[i].Quantity <= 0) slotsList[i] = new InventorySlot(null, 0);
                OnInventoryChanged?.Invoke();
                return;
            }
        }
    }

    public void SaveInventory()
    {
        InventorySaveData data = new InventorySaveData();
        for (int i = 0; i < slotsList.Count; i++)
        {
            if (slotsList[i].Data != null)
            {
                data.savedSlots.Add(new SlotSaveData { itemID = slotsList[i].Data.ID, amount = slotsList[i].Quantity, slotIndex = i });
            }
        }
        PlayerPrefs.SetString(SaveKey, JsonUtility.ToJson(data));
        PlayerPrefs.Save();
    }

    //Callable from "Continue"
    public void LoadInventory()
    {
        if (!PlayerPrefs.HasKey(SaveKey)) return;
        string json = PlayerPrefs.GetString(SaveKey);
        InventorySaveData data = JsonUtility.FromJson<InventorySaveData>(json);
        InitializeInventory();
        foreach (var saved in data.savedSlots)
        {
            ItemData item = allItemsList.Find(it => it.ID == saved.itemID);
            if (item != null && saved.slotIndex < maxSlots)
                slotsList[saved.slotIndex] = new InventorySlot(item, saved.amount);
        }
        OnInventoryChanged?.Invoke();
    }
}

//Auxiliar classes
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
    public int slotIndex;
}