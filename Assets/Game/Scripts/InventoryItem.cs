using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public ItemData itemData;
    [SerializeField] Image iconImage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        iconImage.sprite = itemData.icon;
    }
}
