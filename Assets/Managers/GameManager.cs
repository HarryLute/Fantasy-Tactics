using System;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState GameState;

    private void Awake()
    {
        Instance = this;

        if (GridManager.Instance == null)
        {
            GridManager.Instance = new GridManager(); 
            Debug.LogWarning("GridManager was not initialized. Initializing now.");
        }

        if (UnitManager.Instance == null)
        {
            UnitManager.Instance = new UnitManager(); 
            Debug.LogWarning("UnitManager was not initialized. Initializing now.");
        }

        
    }
    void Start()
    {
        ChangeState(GameState.GenerateGrid);
    }

    public void ChangeState(GameState newState)
    {
        // Debugging to trace execution order
        Debug.Log($"Changing state to: {newState}");

        GameState = newState;

        switch (newState)
        {
            case GameState.GenerateGrid:
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.SpawnHeroes:
                if (GridManager.Instance.GetAllTiles() == null)
                {
                    Debug.LogError("GridManager has not generated the grid yet!");
                    return;
                }
                UnitManager.Instance.SpawnHeroes();
                break;
            case GameState.SpawnEnemies:
                UnitManager.Instance.SpawnEnemies();
                break;
            case GameState.HeroesTurn:
                TurnManager.Instance.StartTurn(GameState.HeroesTurn);
                break;
            case GameState.EnemiesTurn:
                TurnManager.Instance.StartTurn(GameState.EnemiesTurn);
                UnitManager.Instance.PerformEnemyTurn();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }



}
public enum GameState
{
    GenerateGrid = 0,
    SpawnHeroes = 1,
    SpawnEnemies = 2,
    HeroesTurn = 3,
    EnemiesTurn = 4,
}