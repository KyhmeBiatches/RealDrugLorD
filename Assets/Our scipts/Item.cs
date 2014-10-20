using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item 
    {
    public string itemName;
    public int itemID;
    public string itemDesc;
    public Texture2D itemIcon;
    public ItemType itemType;

    public enum ItemType
    {
        Quest,
        Consumable
    }

    public Item()
    {

    }

    public Item(string name, int ID, string desc, ItemType type)
    {
        this.itemName = name;
        this.itemID = ID;
        this.itemDesc = desc;
        itemIcon = Resources.Load<Texture2D>("ItemIcons/" + name);
        this.itemType = type;
    }
}
