using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _grassTile, _mountainTile, _bridgeTile, _waterTile, _treeTile;
    [SerializeField] private Transform _cam;
    private Dictionary<Vector2, Tile> _tiles;
    public CameraController cameraController;



    private void Awake()
    {
        Instance = this;
        CenterCameraOnMap();
        // Optionally, do some early initialization, but avoid complex setup here
    }
    void Start()
    {
        if (cameraController != null)
        {
            cameraController.mapBoundsX = new Vector2(0, tileLayout.GetLength(1) - 1);
            cameraController.mapBoundsY = new Vector2(0, tileLayout.GetLength(0) - 1);
        }
    }

    int[,] tileLayout = new int[,]
{
    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2},
    { 1, 1, 1, 1, 4, 1, 1, 1, 1, 1, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 4, 1, 1, 1, 1, 1, 2, 2, 2, 2},
    { 4, 1, 1, 1, 1, 1, 4, 1, 2, 1, 1, 2, 1, 1, 1, 1, 3, 3, 1, 1, 1, 1, 1, 1, 1, 3, 3, 1, 4, 4, 4, 4, 1, 1, 1, 1, 1, 1},
    { 4, 4, 1, 1, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 2, 2, 2, 1, 3, 1, 1, 4, 4, 4, 4, 1, 1, 1, 1, 1, 1},
    { 4, 4, 1, 1, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1, 1, 3, 3, 3, 1, 1, 1, 2, 1, 1, 1, 1, 3, 1, 1, 1, 4, 4, 4, 4, 4, 1, 1, 1},
    { 4, 4, 1, 1, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1, 1, 3, 3, 1, 1, 1, 1, 2, 1, 1, 1, 1, 3, 3, 1, 1, 4, 4, 4, 4, 4, 1, 1, 1},
    { 4, 4, 1, 1, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 2, 1, 1, 1, 1, 0, 0, 1, 1, 1, 4, 4, 4, 1, 1, 1, 1},
    { 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 3, 3, 1, 1, 1, 1, 2, 2, 2, 1, 1, 3, 3, 1, 1, 1, 4, 4, 4, 1, 1, 1, 1},
    { 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1, 1, 3, 3, 1, 1, 1, 1, 1, 1, 2, 4, 1, 3, 3, 3, 1, 1, 4, 4, 1, 1, 1, 1, 1},
    { 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 2, 4, 1, 1, 3, 3, 1, 1, 1, 1, 1, 1, 1, 1, 2},
    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 1, 1, 1, 1, 1, 1, 2, 4, 1, 1, 1, 3, 1, 1, 1, 1, 3, 3, 3, 3, 2},
    { 1, 4, 4, 4, 4, 4, 1, 1, 4, 1, 1, 2, 2, 2, 2, 3, 3, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 3, 3, 1, 1, 3, 3, 3, 3, 3, 2},
    { 4, 1, 4, 4, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 1, 1, 1, 1, 2, 1, 4, 4, 4, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3},
    { 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 3, 3, 3, 1, 1, 1, 1, 2, 1, 4, 4, 4, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3},
    { 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 1, 1, 1, 1, 1, 3, 3, 1, 1, 1, 1, 1, 1, 4, 4, 4, 4, 4, 4, 4, 1, 1, 3, 3, 3, 3},
    { 4, 4, 4, 2, 4, 4, 4, 1, 1, 1, 3, 3, 3, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 4, 4, 1, 1, 1, 4, 4, 1, 1, 3, 3, 3},
    { 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 3, 3, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1},
    { 1, 1, 1, 1, 2, 1, 4, 4, 1, 1, 1, 3, 3, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 4, 1, 3, 1},
    { 1, 1, 2, 2, 2, 1, 1, 4, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1},
    { 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 4, 4, 4, 1, 1, 3, 3, 3, 3, 3, 1, 1, 1, 1, 3, 3, 1, 1, 1, 1, 1, 1, 1, 3, 1},
    { 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 3, 1},
    { 1, 1, 1, 1, 2, 1, 1, 4, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 1},
    { 1, 1, 1, 1, 2, 1, 1, 4, 4, 4, 4, 3, 3, 0, 3, 3, 3, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 0, 3, 3, 1, 1, 1, 1, 1, 1, 1},
    { 1, 1, 1, 1, 2, 1, 1, 1, 4, 4, 4, 4, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
    { 1, 1, 1, 1, 2, 2, 2, 1, 4, 4, 1, 3, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 4, 1, 1, 1},
    { 4, 1, 1, 1, 1, 1, 2, 1, 1, 1, 3, 1, 1, 1, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
    { 4, 4, 1, 1, 1, 1, 2, 1, 1, 1, 3, 1, 1, 1, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 4, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
    { 4, 4, 4, 4, 1, 1, 2, 1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 4, 4, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
    { 4, 4, 4, 4, 1, 1, 2, 1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 4, 4, 4, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2},
    { 4, 4, 4, 4, 4, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 4, 4, 4, 1, 1, 1, 1, 2, 3},
    { 4, 4, 4, 1, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 4, 4, 4, 4, 4, 1, 1, 1, 1, 1},
    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 4, 4, 4, 4, 4, 1, 1, 1, 1, 2, 3},
    { 2, 2, 2, 2, 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 2, 3, 3},
    { 3, 3, 3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 4, 4, 4, 4, 4, 4, 4, 1, 1, 1},
    { 3, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 3, 3, 1, 1, 1, 1, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 1, 1, 4},
    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 4, 4, 4, 4, 4, 4, 4, 4, 1, 1, 4, 1, 4, 4},
    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 4, 4, 4, 4, 4, 4, 1, 1, 1, 1, 1, 1, 4, 4},
    { 4, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 4, 4, 4, 4, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1},
    { 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
    { 4, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 1},
    { 4, 4, 1, 1, 1, 1, 1, 1, 4, 1, 1, 1, 1, 3, 3, 3, 3, 3, 1, 1, 1, 1, 4, 4, 4, 1, 1, 1, 1, 1, 4, 4, 4, 1, 1, 1, 1, 4},
};
    public void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();

        for (int x = 0; x < tileLayout.GetLength(0); x++)
        {
            for (int y = 0; y < tileLayout.GetLength(1); y++)
            {
                Tile selectedTile = null;

           
                switch (tileLayout[x, y])
                {
                    case 0:
                        selectedTile = _bridgeTile;
                        break;
                    case 1:
                        selectedTile = _grassTile;
                        break;
                    case 2:
                        selectedTile = _mountainTile;
                        break;
                    case 3:
                        selectedTile = _waterTile;
                        break;
                    case 4:
                        selectedTile = _treeTile;
                        break;
                    default:
                        Debug.LogError($"Unknown tile type at {x}, {y}");
                        break;
                }

                if (selectedTile != null)
                {
                    var spawnedTile = Instantiate(selectedTile, new Vector3(x, y), Quaternion.identity);
                    spawnedTile.name = $"Tile {x} {y}";

                    spawnedTile.Init(x, y);

                    _tiles[new Vector2(x, y)] = spawnedTile;
                }
            }

            if (_tiles == null || _tiles.Count == 0)
            {
                Debug.LogError("Grid has not been generated properly!");
                return;
            }
        }
        GameManager.Instance.ChangeState(GameState.SpawnHeroes);
    }


    void HandleCameraMovement()
    {
        float moveSpeed = 5f; // Adjust speed as needed
        float moveX = Input.GetAxis("Horizontal"); // Arrow keys or A/D
        float moveY = Input.GetAxis("Vertical");   // Arrow keys or W/S

        if (_cam != null)
        {
            _cam.position += new Vector3(moveX, moveY, 0) * moveSpeed * Time.deltaTime;
        }
    }

    


    public Tile GetHeroSpawnTile()
    {
        int minX = 0;
        int maxX = 9;
        int minY = 0;
        int maxY = 7;
        return _tiles
            .Where(t => t.Key.x >= minX && t.Key.x <= maxX && t.Key.y >= minY && t.Key.y <= maxY && t.Value.Walkable)
            .OrderBy(t => Random.value) 
            .FirstOrDefault()          
            .Value;
    }

    public Tile GetEnemySpawnTile()
    {
        int heroMinX = 0;
        int heroMaxX = 9;
        int heroMinY = 0;
        int heroMaxY = 7;

        return _tiles
            .Where(t =>
                t.Value.Walkable && 
                (t.Key.x < heroMinX || t.Key.x > heroMaxX ||
                 t.Key.y < heroMinY || t.Key.y > heroMaxY)) 
            .OrderBy(t => Random.value)
            .FirstOrDefault()
            .Value;
    }


    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

    void CenterCameraOnMap()
    {
        float centerX = (tileLayout.GetLength(1) - 1) / 2f;
        float centerY = (tileLayout.GetLength(0) - 1) / 2f;

        if (_cam != null)
        {
            _cam.position = new Vector3(centerX, centerY, _cam.position.z);
        }
    }

    public IEnumerable<Tile> GetAllTiles()
    {
        return _tiles.Values;
    }


}