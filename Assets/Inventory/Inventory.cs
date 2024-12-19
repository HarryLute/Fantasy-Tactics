using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int maxSlots = 5; 
    public List<Item> items = new List<Item>();

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

    public void DisplayInventory()
    {
        Debug.Log("Inventory:");
        foreach (var item in items)
        {
            Debug.Log($"- {item.itemName}");
        }
    }
}

