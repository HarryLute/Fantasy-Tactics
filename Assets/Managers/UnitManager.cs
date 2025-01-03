using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    private List<ScriptableUnit> _units;
    private List<BaseUnit> availablePrefabs = new List<BaseUnit>();

    public BaseHero SelectedHero;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
        }
        else
        {
            Debug.LogWarning("Duplicate UnitManager detected. Destroying the duplicate instance.");
            Destroy(gameObject);
        }
    }

    public void SpawnHeroes(int heroCount = 4)
    {
        for (int i = 0; i < heroCount; i++)
        {
            var randomPrefab = GetRandomUnit<BaseHero>(Faction.Hero);
            if (randomPrefab == null)
            {
                Debug.LogError("Failed to spawn a hero due to missing prefabs.");
                continue;
            }

            var spawnedHero = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetHeroSpawnTile();

            randomSpawnTile.SetUnit(spawnedHero);
        }

        GameManager.Instance.ChangeState(GameState.SpawnEnemies);
    }

    public void SpawnEnemies(int enemyCount = 17)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            var randomPrefab = GetRandomUnit<BaseEnemy>(Faction.Enemy);
            if (randomPrefab == null)
            {
                Debug.LogError("Failed to spawn an enemy due to missing prefabs.");
                continue;
            }

            var spawnedEnemy = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetEnemySpawnTile();

            randomSpawnTile.SetUnit(spawnedEnemy);
        }

        GameManager.Instance.ChangeState(GameState.HeroesTurn);
    }

    private T GetRandomUnit<T>(Faction faction) where T : BaseUnit
    {
        var matchingUnits = _units.Where(u => u.Faction == faction).ToList();

        if (matchingUnits.Count == 0)
        {
            Debug.LogError($"No units found for faction: {faction}");
            return null;
        }

        var randomScriptableUnit = matchingUnits.OrderBy(o => Random.value).First();

        if (randomScriptableUnit.UnitPrefabs.Count == 0)
        {
            Debug.LogError("No prefabs available in the UnitPrefabs list!");
            return null;
        }

        // Refresh availablePrefabs if exhausted
        if (availablePrefabs.Count == 0)
        {
            availablePrefabs.AddRange(randomScriptableUnit.UnitPrefabs);
        }

        var randomPrefab = availablePrefabs[Random.Range(0, availablePrefabs.Count)];
        availablePrefabs.Remove(randomPrefab);
        return randomPrefab as T;
    }

    public void SetSelectedHero(BaseHero hero)
    {
        SelectedHero = hero;
        MenuManager.Instance.ShowSelectedHero(hero);
    }

    public int GetHeroCount()
    {
        return GridManager.Instance.GetAllTiles()
                                   .Where(tile => tile.OccupiedUnit != null && tile.OccupiedUnit.Faction == Faction.Hero)
                                   .Count();
    }

    public int GetEnemyCount()
    {
        return GridManager.Instance.GetAllTiles()
                                   .Where(tile => tile.OccupiedUnit != null && tile.OccupiedUnit.Faction == Faction.Enemy)
                                   .Count();
    }

    public void PerformEnemyTurn()
    {
        var enemies = GetAllEnemies();

        foreach (var enemy in enemies)
        {
            // Perform enemy AI logic (e.g., move towards a target, attack).
            enemy.PerformAction();
        }

        TurnManager.Instance.UnitActionComplete(); // Notify TurnManager after all enemy actions
    }

    public List<BaseEnemy> GetAllEnemies()
    {
        return GridManager.Instance.GetAllTiles()
                                   .Where(tile => tile.OccupiedUnit != null && tile.OccupiedUnit.Faction == Faction.Enemy)
                                   .Select(tile => tile.OccupiedUnit as BaseEnemy)
                                   .ToList();
    }

    public void ResetUnitsActed(Faction faction)
    {
        var unitsToReset = GridManager.Instance.GetAllTiles()
                                               .Where(tile => tile.OccupiedUnit != null && tile.OccupiedUnit.Faction == faction)
                                               .Select(tile => tile.OccupiedUnit);

        foreach (var unit in unitsToReset)
        {
            unit.hasActed = false;
        }

        Debug.Log($"{faction} units have been reset for a new turn.");
    }
}
