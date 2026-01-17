using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string id;
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;
    [SerializeField] private int maxStackSize = 99;
    [SerializeField] private Sprite icon;
    [SerializeField] private bool isStackable;

    public string ID => id;
    public string ItemName => itemName;
    public Sprite Icon => icon;
    public bool IsStackable => isStackable;
    public int MaxStackSize => maxStackSize;
    public string Description => itemDescription;
}