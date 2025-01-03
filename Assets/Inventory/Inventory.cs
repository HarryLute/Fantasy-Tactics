using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int maxSlots = 3; // Max slots for the unit's inventory
    public List<Item> items = new List<Item>(); // List to hold items

    // Add an item to the inventory
    public bool AddItem(Item newItem)
    {
        if (items.Count < maxSlots)
        {
            items.Add(newItem);
            Debug.Log($"{newItem.itemName} added to inventory.");
            return true;
        }
        else
        {
            Debug.Log("Inventory is full!");
            return false;
        }
    }

    // Remove an item from the inventory
    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Debug.Log($"{item.itemName} removed from inventory.");
        }
        else
        {
            Debug.Log($"{item.itemName} not found in inventory.");
        }
    }
}


