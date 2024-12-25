using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item", order = 1)]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public string description;
    public int power;          // Example: Attack power
    public int durability;     // Durability of the item
    public bool isConsumable;
    public int healAmount; // For potions, etc.
    public int WeaponWeight;
}

