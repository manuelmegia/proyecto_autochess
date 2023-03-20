using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _cam;

    private Dictionary<Vector2, Tile> _tiles;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x}, {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);

                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

    public List<Tile> FindPath(Vector2 startPos, Vector2 targetPos)
    {
        var startNode = new Node(startPos);
        var targetNode = new Node(targetPos);

        var openList = new List<Node> { startNode };
        var closedList = new List<Node>();

        while (openList.Count > 0)
        {
            // Find the node with the lowest F value
            var currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].F < currentNode.F)
                {
                    currentNode = openList[i];
                }
            }

            // Move the current node from the open list to the closed list
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // If we've found the target node, construct the path and return it
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

            // Expand the current node and add its neighbors to the open list
            foreach (var neighborPos in GetNeighborPositions(currentNode.Position))
            {
                // If the neighbor is already in the closed list, skip it
                if (closedList.Exists(node => node.Position == neighborPos))
                {
                    continue;
                }

                var neighborNode = openList.Find(node => node.Position == neighborPos);
                if (neighborNode == null)
                {
                    // If the neighbor is not already in the open list, add it
                    neighborNode = new Node(neighborPos, currentNode);
                    openList.Add(neighborNode);
                }
                else
                {
                    // If the neighbor is already in the open list, check if the current path is better than the existing path
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
        // If we've searched the entire grid and haven't found a path, return null
        return null;
    }

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

    private float CalculateHCost(Vector2 pos, Vector2 targetPos)
    {
        // Use the Manhattan distance as the heuristic
        return Mathf.Abs(pos.x - targetPos.x) + Mathf.Abs(pos.y - targetPos.y);
    }
}
