using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    private bool _isOffset;

    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }

    public void SetColor(Color color)
    {
        _renderer.color = color;
    }

    public bool IsOffset { get { return _isOffset; } }
    
    //

    public void OnMouseDown()
    {
        var gridManager = FindObjectOfType<GridManager>();

        // Encontrar los Tiles en las posiciones de inicio y destino
        var startTile = gridManager.GetTileAtPosition(new Vector2(0, 0));
        var tilePosition = gridManager.GetTileAtPosition(transform.position);

        // Encontrar la ruta óptima entre los Tiles
        var path = gridManager.FindPath(startTile.transform.position, tilePosition.transform.position);

        if (path != null)
        {
            // Cambiamos el color de los Tile en el camino
            foreach (var tile in path)
            {
                tile.SetColor(Color.green);
            }
        }
        else
        {
            Debug.Log("No se encontró un camino desde la posición de inicio hasta la posición de destino.");
        }
    }

    public void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }

    public void OnMouseExit()
    {
        _highlight.SetActive(false);
    }
}