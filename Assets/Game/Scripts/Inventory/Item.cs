using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class Item : ScriptableObject
{
    private readonly string itemName;
    private readonly string itemDescription;
    private readonly int maxStackSize = 99;
    private Sprite icon;

    public string ItemName => itemName;
    public string ItemDescription => itemDescription;
    public int MaxStackSize => maxStackSize;
    public Sprite Icon => icon;
}