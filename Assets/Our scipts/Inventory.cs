using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
    public int slotsX, slotsY;
    public GUISkin skin;
    public List<Item> inventory = new List<Item>();
    public List<Item> slots = new List<Item>();
    private bool showInventory;
    private ItemDatabase database;
    private bool showTooltip;
    private string tooltip;

    private bool draggingItem;
    private Item draggedItem;
    private int prevIndex;

	// Use this for initialization
	void Start () {
        for(int i = 0; i < (slotsX*slotsY); i++)
        {
            slots.Add(new Item());
            inventory.Add(new Item());
        }
        database = GameObject.FindGameObjectWithTag("Item Database").GetComponent<ItemDatabase>();
        AddItem(0);
        AddItem(1);
        AddItem(2);
	}
	
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            showInventory = !showInventory;
        }
    }

	void OnGUI () 
    {
        tooltip = "";
        GUI.skin = skin;
        if (showInventory)
        {
            DrawInventory();

            if (showTooltip)
            {
                GUI.Box(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 200, 200), tooltip, skin.GetStyle("Tooltip"));
            }
            if (tooltip == "")
            {
                showTooltip = false;
            }
        }
        if(draggingItem)
        {
            GUI.DrawTexture(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 50, 50), draggedItem.itemIcon);
        }
	}

    void DrawInventory()
    {
        Event e = Event.current;
        int i = 0;
        for(int y = 0; y < slotsY; y++)
        {
            for(int x = 0; x < slotsX; x++)
            {
                Rect slotRect = new Rect(x * 33, y * 33, 30, 30);
                GUI.Box(slotRect, "", skin.GetStyle("Slot"));
                this.slots[i] = this.inventory[i];
                if (this.slots[i].itemIcon != null)
                {
                    GUI.DrawTexture(slotRect, slots[i].itemIcon);
                    if(slotRect.Contains(e.mousePosition))
                    {
                        CreateTooltip(slots[i]);
                        showTooltip = true;
                        if(e.button == 0 && e.type == EventType.mouseDrag && !draggingItem)
                        {
                            draggingItem = true;
                            prevIndex = i;
                            draggedItem = slots[i];
                            inventory[i] = new Item();
                        }
                        if(e.type == EventType.mouseUp && draggingItem)
                        {
                            inventory[prevIndex] = inventory[i];
                            inventory[i] = draggedItem;
                            draggingItem = false;
                            draggedItem = null;
                        }
                    }
                }
                else
                {
                    if (slotRect.Contains(e.mousePosition))
                    {
                        if (e.type == EventType.mouseUp && draggingItem)
                        {
                            inventory[i] = draggedItem;
                            draggingItem = false;
                            draggedItem = null;
                        }
                    }
                }


                i++;
            }
        }
    }

    string CreateTooltip(Item item)
    {
        tooltip = "<color=#ffffff>" + item.itemName + "</Color>\n\n" + item.itemDesc;
        Debug.Log(Parley.GetInstance().GetCurrentQuests().Count);
        return tooltip;
    }

    void AddItem(int id)
    {
        for (int i = 0; i < database.items.Count; i++)
        {
            if(id == i)
            {
                 for(int j = 0; j < slots.Count; j++)
                 {
                     if(inventory[j].itemName == null)
                     {
                         inventory[j] = database.items[i];
                         break;
                     }
                 }
            }
        }
    }

    void AddQuestItem(int id)
    {
        Debug.Log(InventoryContains(id));
        if (!InventoryContains(id))
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].itemName == null)
                {
                    inventory[i] = database.items[id];
                    break;
                }
            }
        }
    }

    bool InventoryContains(int id)
    {
        bool result = false;
        for (int i = 0; i < inventory.Count; i++)
        {
            result = (inventory[i].itemID == id);
            if(result)
            {
                break;
            }
        }
        return result;
    }

    void RemoveItem(int id)
    {
        for(int i = 0; i < inventory.Count; i++)
        {
            if(inventory[i].itemID == id)
            {
                inventory[i] = new Item();
                break;
            }
        }
    }


}
