// Importaci�n de librer�as necesarias
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Declaraci�n de la clase Tile, que hereda de MonoBehaviour
public class Tile : MonoBehaviour
{
    // Declaraci�n de variables que se mostrar�n en el Inspector de Unity
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    private bool _isOffset;
    //a�adido nuevo
    public GameObject personaje;
    // M�todo de inicializaci�n de un Tile con offset
    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }

    // M�todo para establecer el color de un Tile
    public void SetColor(Color color)
    {
        _renderer.color = color;
    }

    // Propiedad para obtener si un Tile est� en offset o no
    public bool IsOffset { get { return _isOffset; } }

    // M�todo que se ejecuta cuando se hace click en un Tile
    public void OnMouseDown()
    {
        // Buscamos el objeto GridManager en la escena
        var gridManager = FindObjectOfType<GridManager>();

        // Obtenemos el Tile de inicio para encontrar el camino �ptimo
        Tile startTile;
        if (gridManager._lastTile == null)
        {
            startTile = gridManager.GetTileAtPosition(new Vector2(0, 0));
        }
        else
        {
            startTile = gridManager._lastTile;
        }

        // Obtenemos la posici�n del Tile actual
        var tilePosition = gridManager.GetTileAtPosition(transform.position);

        // Encontramos la ruta �ptima entre los Tiles
        var path = gridManager.FindPath(startTile.transform.position, tilePosition.transform.position);

        if (path != null)
        {
            // Cambiamos el color de los Tiles en el camino a verde
            foreach (var tile in path)
            {
                tile.SetColor(Color.green);
            }
        }
        else
        {
            Debug.Log("No se encontr� un camino desde la posici�n de inicio hasta la posici�n de destino.");
        }

        // Actualizamos el �ltimo Tile seleccionado
        gridManager._lastTile = tilePosition;
    }

    // M�todo que se ejecuta cuando el mouse entra en un Tile
    public void OnMouseEnter()
    {
        // Activamos el objeto que representa el resaltado
        _highlight.SetActive(true);
    }

    // M�todo que se ejecuta cuando el mouse sale de un Tile
    public void OnMouseExit()
    {
        // Desactivamos el objeto que representa el resaltado
        _highlight.SetActive(false);
    }

}