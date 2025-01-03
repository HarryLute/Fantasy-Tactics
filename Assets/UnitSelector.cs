using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    public InventoryUI inventoryUI; // Reference to InventoryUI

    public void OnUnitSelected(BaseUnit unit)
    {
        if (unit == null)
        {
            inventoryUI.ClearInventoryUI(); 
            return;
        }

        
        Inventory unitInventory = unit.GetComponent<Inventory>();

        if (unitInventory != null)
        {
            inventoryUI.UpdateInventoryUI(unitInventory); // Update UI with the selected unit's inventory
        }
        else
        {
            Debug.LogWarning("Selected unit does not have an Inventory component.");
            inventoryUI.ClearInventoryUI(); 
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                BaseUnit unit = hit.collider.GetComponent<BaseUnit>();
                OnUnitSelected(unit); 
            }
        }
    }
}



