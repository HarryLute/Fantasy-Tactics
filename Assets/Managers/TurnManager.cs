using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    private int unitsRemaining;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log($"{nameof(TurnManager)} initialized.");
        }
        else
        {
            Debug.LogWarning($"{nameof(TurnManager)} already has an instance. Destroying duplicate!");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (TurnManager.Instance == null)
        {
            Debug.LogError("TurnManager is not initialized before GameManager starts!");
        }
        else
        {
            Debug.Log("TurnManager is initialized.");
        }

        GameManager.Instance.ChangeState(GameState.HeroesTurn);
    }

    public void StartTurn(GameState gameState)
    {
        if (gameState == GameState.HeroesTurn)
        {
            // Reset "hasActed" for all heroes
            UnitManager.Instance.ResetUnitsActed(Faction.Hero);
            unitsRemaining = UnitManager.Instance.GetHeroCount();
        }
        else if (gameState == GameState.EnemiesTurn)
        {
            // Reset "hasActed" for all enemies
            UnitManager.Instance.ResetUnitsActed(Faction.Enemy);
            unitsRemaining = UnitManager.Instance.GetEnemyCount();
        }

        Debug.Log($"Starting {gameState}, units to act: {unitsRemaining}");
    }

    public void UnitActionComplete()
    {
        unitsRemaining--;

        if (unitsRemaining <= 0)
        {
            EndTurn();
        }
    }

    private void EndTurn()
    {
        if (GameManager.Instance.GameState == GameState.HeroesTurn)
        {
            GameManager.Instance.ChangeState(GameState.EnemiesTurn);
        }
        else if (GameManager.Instance.GameState == GameState.EnemiesTurn)
        {
            GameManager.Instance.ChangeState(GameState.HeroesTurn);
        }
    }
}
