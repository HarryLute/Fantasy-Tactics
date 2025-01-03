using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel; // The panel showing the inventory
    public Text[] itemSlots;         // Array of UI Text elements for displaying inventory items

    // Update the inventory UI based on the given unit's inventory
    public void UpdateInventoryUI(Inventory inventory)
    {
        if (inventory == null)
        {
            Debug.LogWarning("Inventory is null. Clearing the inventory UI.");
            ClearInventoryUI();
            return;
        }

        if (itemSlots == null || itemSlots.Length == 0)
        {
            Debug.LogError("ItemSlots array is null or empty. Make sure it is set in the Inspector.");
            return;
        }

        // Update each item slot with inventory data
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                if (itemSlots[i] == null)
                {
                    Debug.LogError($"ItemSlot at index {i} is null. Check your UI setup.");
                    continue;
                }
                itemSlots[i].text = inventory.items[i].itemName;  // Update the slot with the item name
            }
            else
            {
                if (itemSlots[i] != null)
                {
                    itemSlots[i].text = "";  // Clear slot if no item exists
                }
            }
        }
    }

    // Clears all inventory slots in the UI
    public void ClearInventoryUI()
    {
        if (itemSlots == null || itemSlots.Length == 0)
        {
            Debug.LogError("ItemSlots array is null or empty. Make sure it is set in the Inspector.");
            return;
        }

        foreach (var slot in itemSlots)
        {
            if (slot != null)
            {
                slot.text = "";  // Clear each item slot
            }
        }
    }
}


