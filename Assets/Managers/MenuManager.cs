using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] private GameObject _selectedHeroObject, _tileObject, _tileUnitObject;

    void Awake()
    {
        // Singleton pattern initialization
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate instances
        }
    }

    public void ShowTileInfo(Tile tile)
    {
        // If the tile is null, hide both objects and return early
        if (tile == null)
        {
            if (_tileObject != null) _tileObject.SetActive(false);
            if (_tileUnitObject != null) _tileUnitObject.SetActive(false);
            return;
        }

        // Safely handle UI element updates
        if (_tileObject != null && _tileObject.GetComponentInChildren<Text>() != null)
        {
            _tileObject.GetComponentInChildren<Text>().text = tile.TileName;
            _tileObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("TileObject or its Text component is not assigned.");
        }

        // Handle occupied unit UI
        if (tile.OccupiedUnit != null)
        {
            if (_tileUnitObject != null && _tileUnitObject.GetComponentInChildren<Text>() != null)
            {
                _tileUnitObject.GetComponentInChildren<Text>().text = tile.OccupiedUnit.UnitName;
                _tileUnitObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("TileUnitObject or its Text component is not assigned.");
            }
        }
        else
        {
            if (_tileUnitObject != null)
            {
                _tileUnitObject.SetActive(false);
            }
        }
    }

    public void ShowSelectedHero(BaseHero hero)
    {
        if (hero == null)
        {
            if (_selectedHeroObject != null)
                _selectedHeroObject.SetActive(false);
            return;
        }

        // Safely handle UI element updates for the selected hero
        if (_selectedHeroObject != null && _selectedHeroObject.GetComponentInChildren<Text>() != null)
        {
            _selectedHeroObject.GetComponentInChildren<Text>().text = hero.UnitName;
            _selectedHeroObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("SelectedHeroObject or its Text component is not assigned.");
        }
    }

}

