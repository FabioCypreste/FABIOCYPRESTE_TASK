using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;
    [SerializeField] private int maxStackSize = 99;
    [SerializeField] private Sprite icon;

    public string ItemName => itemName;
    public string ItemDescription => itemDescription;
    public int MaxStackSize => maxStackSize;
    public Sprite Icon => icon;
}