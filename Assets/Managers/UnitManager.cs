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

    void Awake()
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
        var enemyCount = 1;

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
}