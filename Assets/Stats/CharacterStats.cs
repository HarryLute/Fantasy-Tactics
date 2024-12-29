using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    public string characterName;

    // Base stats
    public int level;
    public int maxHP;
    public int currentHP;
    public int strength;
    public int magic;
    public int defense;
    public int resistance;
    public int speed;
    public int skill;

    // Growth rates (optional, for Fire Emblem-style level-ups)
    [Range(0, 100)] public int hpGrowth;
    [Range(0, 100)] public int strengthGrowth;
    [Range(0, 100)] public int magicGrowth;
    [Range(0, 100)] public int defenseGrowth;
    [Range(0, 100)] public int resistanceGrowth;
    [Range(0, 100)] public int speedGrowth;
    [Range(0, 100)] public int skillGrowth;
}

