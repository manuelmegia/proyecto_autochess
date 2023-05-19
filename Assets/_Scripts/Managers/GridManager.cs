// Importar las librerías necesarias
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Definición de la clase GridManager que hereda de MonoBehaviour
public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    // Variables que se pueden modificar desde el inspector de Unity
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _cam;
    // Variables privadas de la clase
    public Dictionary<Vector2, Tile> _tiles;
    public Dictionary<Vector2, Tile> _benchTiles = new Dictionary<Vector2, Tile>();
    public Tile _lastTile;
    public GameObject personaje;

    void Awake()
    {
        Instance = this;
        
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Bench");
        foreach (GameObject tileGameObject in tiles)
        {
            Vector2 position = new Vector2(tileGameObject.transform.position.x, tileGameObject.transform.position.y);
            Tile tileComponent = tileGameObject.GetComponent<Tile>();
            _benchTiles[position] = tileComponent;
        }

    }
    // Método Start que se ejecuta al inicio
    

    // Método que genera la cuadrícula de Tiles
    public void GenerateGrid()
    {
        // Se inicializa el diccionario de Tiles
        _tiles = new Dictionary<Vector2, Tile>();

        // Ciclos para crear cada Tile y añadirlo al diccionario
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x}, {y}";

                // Se determina si el Tile tiene un offset
                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);

                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        // Se ajusta la posición de la cámara para que muestre toda la cuadrícula
        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
        GameManager.Instance.ChangeState(GameState.SpawnHeroes);
    }

    public Tile GetHeroSpawnTile()
    {
        //return _tiles.Where(t=>t.Key.y < _height / 2 && t.Value.Walkable).OrderBy(t=>Random.value).First().Value;
        return _benchTiles.Where(t=> t.Value.Walkable).OrderBy(t=>Random.value).First().Value;
    }
    public Tile GetEnemySpawnTile()
    {
        return _tiles.Where(t=>t.Key.y > _height / 2 && t.Value.Walkable).OrderBy(t=>Random.value).First().Value;
    }
    // Método que devuelve el Tile en una posición dada
    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

    // Método que encuentra el camino más corto entre dos posiciones en la cuadrícula

    public List<Tile> FindPath(Vector2 startPos, Vector2 targetPos)
    {
        // Creamos un nodo inicial y un nodo objetivo
        var startNode = new Node(startPos);
        var targetNode = new Node(targetPos);

        // Creamos una lista abierta que contiene el nodo inicial y una lista cerrada vacía
        var openList = new List<Node> { startNode };
        var closedList = new List<Node>();

        // Mientras haya nodos en la lista abierta
        while (openList.Count > 0)
        {
            // Seleccionamos el nodo con el valor F más bajo
            var currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].F < currentNode.F)
                {
                    currentNode = openList[i];
                }
            }

            // Lo eliminamos de la lista abierta y lo añadimos a la lista cerrada
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // Si hemos llegado al nodo objetivo, reconstruimos el camino y lo devolvemos
            if (currentNode.Position == targetNode.Position)
            {
                var path = new List<Tile>();
                var node = currentNode;
                while (node != null)
                {
                    path.Add(_tiles[node.Position]);
                    node = node.Parent;
                }
                path.Reverse();
                return path;
            }

            // Para cada vecino del nodo actual
            foreach (var neighborPos in GetNeighborPositions(currentNode.Position))
            {
                // Si ya está en la lista cerrada, lo ignoramos
                if (closedList.Exists(node => node.Position == neighborPos))
                {
                    continue;
                }

                // Si no está en la lista abierta, lo añadimos
                var neighborNode = openList.Find(node => node.Position == neighborPos);
                if (neighborNode == null)
                {
                    neighborNode = new Node(neighborPos, currentNode);
                    openList.Add(neighborNode);
                }
                // Si ya está en la lista abierta, comprobamos si llegar a él desde el nodo actual es más rápido que la ruta actual
                else
                {
                    var newG = currentNode.G + 1;
                    if (newG < neighborNode.G)
                    {
                        neighborNode.Parent = currentNode;
                        neighborNode.G = newG;
                        neighborNode.F = newG + CalculateHCost(neighborPos, targetNode.Position);
                    }
                }
            }
        }

        // Si no se encuentra ningún camino, devolvemos null
        return null;
    }

    // Devuelve las posiciones vecinas de una posición dada
    private List<Vector2> GetNeighborPositions(Vector2 pos)
    {
        var result = new List<Vector2>();
        result.Add(pos + Vector2.up);
        result.Add(pos + Vector2.down);
        result.Add(pos + Vector2.left);
        result.Add(pos + Vector2.right);

        result.Add(pos + Vector2.up + Vector2.left);
        result.Add(pos + Vector2.up + Vector2.right);
        result.Add(pos + Vector2.down + Vector2.left);
        result.Add(pos + Vector2.down + Vector2.right);

        return result;
    }

    // Calcula la distancia heurística desde una posición dada hasta el objetivo
    private float CalculateHCost(Vector2 pos, Vector2 targetPos)
    {
        return Mathf.Abs(pos.x - targetPos.x) + Mathf.Abs(pos.y - targetPos.y);
    }
}
