using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel; // Reference to the inventory UI panel
    public Text[] itemSlots;         // Array of UI Text elements for displaying inventory items

    private Inventory currentInventory; // The currently assigned inventory

  
    /// <param name="inventory">The inventory to display.</param>
    public void UpdateInventoryUI(Inventory inventory)
    {
        if (inventory == null)
        {
            Debug.LogWarning("Inventory is null. Clearing the inventory UI.");
            ClearInventoryUI();
            return;
        }

        currentInventory = inventory;

        if (itemSlots == null || itemSlots.Length == 0)
        {
            Debug.LogError("ItemSlots array is null or empty. Make sure it is set in the Inspector.");
            return;
        }

        // Update each item slot with inventory data
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (i < currentInventory.items.Count)
            {
                if (itemSlots[i] == null)
                {
                    Debug.LogError($"ItemSlot at index {i} is null. Check your UI setup.");
                    continue;
                }
                itemSlots[i].text = currentInventory.items[i].itemName;
            }
            else
            {
                if (itemSlots[i] != null)
                {
                    itemSlots[i].text = "";
                }
            }
        }
    }

    /// <summary>
    /// Clears all inventory slots in the UI.
    /// </summary>
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
                slot.text = "";
            }
        }

        currentInventory = null; // Clear the reference to the inventory
    }
}

