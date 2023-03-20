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
        var targetTile = gridManager.GetTileAtPosition(new Vector2(7, 3));

        // Encontrar la ruta óptima entre los Tiles
        var path = gridManager.FindPath(startTile.transform.position, targetTile.transform.position);

        // Imprimir la ruta en la consola
        //Toast.Show();
        // Llamamos a la función FindPath del GridManager con la posición de inicio y la posición de destino

        /*var path = _gridManager.FindPath(( x: 0, y: 0 ), ( x: 2, y: 5 ));
        */
        // Verificamos si se encontró un camino
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