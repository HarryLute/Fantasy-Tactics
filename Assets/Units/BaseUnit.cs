using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    public string UnitName;
    public Tile OccupiedTile;
    public Faction Faction;

    public Inventory inventory;
    public Item equippedWeapon;

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
}
