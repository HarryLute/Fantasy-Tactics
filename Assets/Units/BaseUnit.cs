using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    public string UnitName;
    public Tile OccupiedTile;
    public Faction Faction;

    public Inventory inventory;
    public Item equippedWeapon;
    public CharacterStats stats;

    public bool hasActed = false;

    public void PerformAction()
    {
        if (!hasActed)
        {
          
            Debug.Log($"{gameObject.name} performed an action.");

            hasActed = true;
            TurnManager.Instance.UnitActionComplete();
        }
        else
        {
            Debug.Log($"{gameObject.name} has already acted.");
        }
    }

    public bool CanAttack(BaseUnit targetUnit)
    {
        // Ensure a weapon is equipped
        if (equippedWeapon == null)
        {
            Debug.LogError($"{UnitName} has no weapon equipped!");
            return false;
        }

        // Calculate Manhattan distance
        int distance = Mathf.Abs((int)(OccupiedTile.transform.position.x - targetUnit.OccupiedTile.transform.position.x)) +
                       Mathf.Abs((int)(OccupiedTile.transform.position.y - targetUnit.OccupiedTile.transform.position.y));

        // Check if the target is within range
        return distance <= equippedWeapon.range;
    }

    public void Attack(BaseUnit targetUnit)
    {
        if (CanAttack(targetUnit))
        {
            Debug.Log($"{UnitName} attacked {targetUnit.UnitName}!");

            // Add damage or destroy logic here
            Destroy(targetUnit.gameObject);

            PerformAction();
        }
        else
        {
            Debug.Log($"{targetUnit.UnitName} is out of range!");
        }
    }

    public void TakeDamage(int damage)
    {
        stats.currentHP -= damage;
        if (stats.currentHP <= 0)
        {
            stats.currentHP = 0;
            Debug.Log($"{stats.characterName} has fallen!");
        }
    }

    private void Update()
    {
        // Open inventory when Q is pressed
        if (Input.GetKeyDown(KeyCode.Q) && UnitManager.Instance.SelectedHero != null)
        {
            var selectedHero = UnitManager.Instance.SelectedHero;
            var heroInventory = selectedHero.GetComponent<Inventory>();
            var inventoryUI = Object.FindFirstObjectByType<InventoryUI>();

            if (inventoryUI != null && heroInventory != null)
            {
                inventoryUI.UpdateInventoryUI(heroInventory); // Pass the selected hero's inventory
                inventoryUI.inventoryPanel.SetActive(true);    // Show the inventory panel
            }
        }

        // Close inventory when ESC is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            var inventoryUI = Object.FindFirstObjectByType<InventoryUI>();
            if (inventoryUI != null && inventoryUI.inventoryPanel.activeSelf)
            {
                inventoryUI.inventoryPanel.SetActive(false); // Hide the inventory panel
            }
        }
    }
}
