using UnityEngine;
using UnityEngine.UIElements;

public abstract class Tile : MonoBehaviour
{
    public string TileName;
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private bool _isWalkable;

    public BaseUnit OccupiedUnit;
    public bool Walkable => _isWalkable && OccupiedUnit == null;

    public virtual void Init(int x, int y)
    {
        
    }
    void OnMouseEnter()
    {
        _highlight.SetActive(true);
        MenuManager.Instance.ShowTileInfo(this);
    }

    public void ClearUnit()
    {
        OccupiedUnit = null;
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
        MenuManager.Instance.ShowTileInfo(null);
    }

    void OnMouseDown()
    {
        if (GameManager.Instance.GameState != GameState.HeroesTurn) return;

        if (OccupiedUnit != null)
        {
            HandleOccupiedUnit();
        }
        else
        {
            HandleEmptyTile();
        }

        CenterCameraOnThisTile();
    }

    private void HandleOccupiedUnit()
    {
        if (OccupiedUnit.Faction == Faction.Hero)
        {
            UnitManager.Instance.SetSelectedHero((BaseHero)OccupiedUnit);
        }
        else
        {
            // Attack the enemy if a hero is selected
            if (UnitManager.Instance.SelectedHero != null)
            {
                var hero = UnitManager.Instance.SelectedHero;
                var enemy = (BaseEnemy)OccupiedUnit;

                if (hero.CanAttack(enemy))
                {
                    hero.Attack(enemy);

                    // Deselect the hero after the attack
                    UnitManager.Instance.SetSelectedHero(null);
                }
                else
                {
                    Debug.Log($"{enemy.UnitName} is out of range!");
                }
            }
        }
    }

    private void HandleEmptyTile()
    {
        // Move the selected hero to this tile
        if (UnitManager.Instance.SelectedHero != null)
        {
            var hero = UnitManager.Instance.SelectedHero;

            // Update hero's position
            hero.OccupiedTile.ClearUnit();
            SetUnit(hero);

            // Complete the hero's action
            hero.PerformAction();

            // Deselect the hero
            UnitManager.Instance.SetSelectedHero(null);
        }
    }

    private void CenterCameraOnThisTile()
    {
        Vector2Int tilePosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);

        CameraController cameraController = Object.FindFirstObjectByType<CameraController>();
        if (cameraController != null)
        {
            cameraController.CenterCameraOnTile(tilePosition);
        }
    }




    public void SetUnit(BaseUnit unit)
    {
        if (unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;
        unit.transform.position = transform.position;
        OccupiedUnit = unit;
        unit.OccupiedTile = this;
    }    
}


