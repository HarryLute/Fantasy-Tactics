using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    private List<ScriptableUnit> _units;

    public BaseHero SelectedHero;

    

    
    private void Awake()
    {
        Instance = this;
        _units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
       
    }


    public void SpawnHeroes()
    {
        var heroCount = 4;

        for (int i = 0; i < heroCount; i++)
        {
            var randomPrefab = GetRandomUnit<BaseHero>(Faction.Hero);
            var spawnedHero = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetHeroSpawnTile();

            randomSpawnTile.SetUnit(spawnedHero);
        }

        GameManager.Instance.ChangeState(GameState.SpawnEnemies);
    }

    public void SpawnEnemies()
    {
        var enemyCount = 17;

        for (int i = 0; i < enemyCount; i++)
        {
            var randomPrefab = GetRandomUnit<BaseEnemy>(Faction.Enemy);
            var spawnedEnemy = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetEnemySpawnTile();

            randomSpawnTile.SetUnit(spawnedEnemy);
        }
        GameManager.Instance.ChangeState(GameState.HeroesTurn);
    }

    private List<BaseUnit> availablePrefabs = new List<BaseUnit>();
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
        if (availablePrefabs.Count == 0)
        {
            availablePrefabs.AddRange(randomScriptableUnit.UnitPrefabs);
        }

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
        // Count heroes on the grid
        return GridManager.Instance.GetAllTiles()
                                   .Where(tile => tile.OccupiedUnit != null && tile.OccupiedUnit.Faction == Faction.Hero)
                                   .Count();
    }

    public int GetEnemyCount()
    {
        // Count enemies on the grid
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
    }

    public List<BaseEnemy> GetAllEnemies()
    {
        // Get all tiles from the GridManager
        var tiles = GridManager.Instance.GetAllTiles();

        // Filter tiles to find enemies and return them as a list
        return tiles
            .Where(tile => tile.OccupiedUnit != null && tile.OccupiedUnit.Faction == Faction.Enemy)
            .Select(tile => tile.OccupiedUnit as BaseEnemy)
            .ToList();
    }


}
