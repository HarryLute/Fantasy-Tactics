using UnityEngine;

public class BaseHero : BaseUnit
{
    public CharacterStats stats;
    public void LevelUp()
    {
        stats.level++;

        if (Random.Range(0, 100) < stats.hpGrowth)
            stats.maxHP += 1;

        if (Random.Range(0, 100) < stats.strengthGrowth)
            stats.strength += 1;

        if (Random.Range(0, 100) < stats.magicGrowth)
            stats.magic += 1;

        if (Random.Range(0, 100) < stats.defenseGrowth)
            stats.defense += 1;

        if (Random.Range(0, 100) < stats.resistanceGrowth)
            stats.resistance += 1;

        if (Random.Range(0, 100) < stats.speedGrowth)
            stats.speed += 1;

        if (Random.Range(0, 100) < stats.skillGrowth)
            stats.skill += 1;

        Debug.Log($"{stats.characterName} leveled up to {stats.level}!");
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

   


    public void Equip(Item weapon)
    {
        if (weapon == null || weapon.isConsumable)
        {
            Debug.Log("This item cannot be equipped.");
            return;
        }

        equippedWeapon = weapon;
        Debug.Log($"{stats.characterName} equipped {weapon.itemName}.");
    }

    public void UseItem(Item consumable)
    {
        if (!consumable.isConsumable)
        {
            Debug.Log("This item is not consumable.");
            return;
        }

        if (consumable.healAmount > 0)
        {
            Heal(consumable.healAmount);
        }

        inventory.RemoveItem(consumable);
        Debug.Log($"{stats.characterName} used {consumable.itemName}.");
    }

    private void Heal(int amount)
    {
        stats.currentHP = Mathf.Min(stats.maxHP, stats.currentHP + amount);
        Debug.Log($"{stats.characterName} healed for {amount}. Current HP: {stats.currentHP}.");
    }
}
