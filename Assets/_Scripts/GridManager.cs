// Importar las librer�as necesarias
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Definici�n de la clase GridManager que hereda de MonoBehaviour
public class GridManager : MonoBehaviour
{
    // Variables que se pueden modificar desde el inspector de Unity
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _cam;
    // Variables privadas de la clase
    private Dictionary<Vector2, Tile> _tiles;
    public Tile _lastTile;

    // M�todo Start que se ejecuta al inicio
    void Start()
    {
        GenerateGrid();
    }

    // M�todo que genera la cuadr�cula de Tiles
    void GenerateGrid()
    {
        // Se inicializa el diccionario de Tiles
        _tiles = new Dictionary<Vector2, Tile>();

        // Ciclos para crear cada Tile y a�adirlo al diccionario
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

        // Se ajusta la posici�n de la c�mara para que muestre toda la cuadr�cula
        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }

    // M�todo que devuelve el Tile en una posici�n dada
    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

    // M�todo que encuentra el camino m�s corto entre dos posiciones en la cuadr�cula

    public List<Tile> FindPath(Vector2 startPos, Vector2 targetPos)
    {
        // Creamos un nodo inicial y un nodo objetivo
        var startNode = new Node(startPos);
        var targetNode = new Node(targetPos);

        // Creamos una lista abierta que contiene el nodo inicial y una lista cerrada vac�a
        var openList = new List<Node> { startNode };
        var closedList = new List<Node>();

        // Mientras haya nodos en la lista abierta
        while (openList.Count > 0)
        {
            // Seleccionamos el nodo con el valor F m�s bajo
            var currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].F < currentNode.F)
                {
                    currentNode = openList[i];
                }
            }

            // Lo eliminamos de la lista abierta y lo a�adimos a la lista cerrada
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
                // Si ya est� en la lista cerrada, lo ignoramos
                if (closedList.Exists(node => node.Position == neighborPos))
                {
                    continue;
                }

                // Si no est� en la lista abierta, lo a�adimos
                var neighborNode = openList.Find(node => node.Position == neighborPos);
                if (neighborNode == null)
                {
                    neighborNode = new Node(neighborPos, currentNode);
                    openList.Add(neighborNode);
                }
                // Si ya est� en la lista abierta, comprobamos si llegar a �l desde el nodo actual es m�s r�pido que la ruta actual
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

        // Si no se encuentra ning�n camino, devolvemos null
        return null;
    }

    // Devuelve las posiciones vecinas de una posici�n dada
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

    // Calcula la distancia heur�stica desde una posici�n dada hasta el objetivo
    private float CalculateHCost(Vector2 pos, Vector2 targetPos)
    {
        return Mathf.Abs(pos.x - targetPos.x) + Mathf.Abs(pos.y - targetPos.y);
    }
}
