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
            
            if (OccupiedUnit.Faction == Faction.Hero)
            {
                UnitManager.Instance.SetSelectedHero((BaseHero)OccupiedUnit);
            }
            else
            {
                
                if (UnitManager.Instance.SelectedHero != null)
                {
                    var enemy = (BaseEnemy)OccupiedUnit;

                    
                    Destroy(enemy.gameObject);

                    UnitManager.Instance.SetSelectedHero(null);

                    TurnManager.Instance.UnitActionComplete();
                }
            }
        }
        else
        {
            // Handle Hero Movement
            if (UnitManager.Instance.SelectedHero != null)
            {
                SetUnit(UnitManager.Instance.SelectedHero);
                UnitManager.Instance.SetSelectedHero(null);

                TurnManager.Instance.UnitActionComplete();
            }
        }
        Vector2Int tilePosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);

        // Use the new FindFirstObjectByType method
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


