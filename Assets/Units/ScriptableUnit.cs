using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Scriptable Unit")]

public class ScriptableUnit : ScriptableObject
{
    public Faction Faction;
    public List<BaseUnit> UnitPrefabs;
}

public enum Faction
{
    Hero = 0,
    Enemy = 1,
}
