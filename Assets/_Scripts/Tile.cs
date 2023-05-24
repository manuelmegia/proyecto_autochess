// Importación de librerías necesarias
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Declaración de la clase Tile, que hereda de MonoBehaviour
public class Tile : MonoBehaviour
{
    // Declaración de variables que se mostrarán en el Inspector de Unity
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private bool _isWalkable;
    
    public BaseUnit OccupiedUnit;
    public bool Walkable => _isWalkable && OccupiedUnit == null;
    private bool _isOffset;
    //añadido nuevo
    public GameObject personaje;
    public bool Reserved { get; set; }
    // Método de inicialización de un Tile con offset
    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }

    /*public void SetUnitSpawn(BaseUnit unit)
    {
        if (unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;
        unit.transform.position = transform.position;
        OccupiedUnit = unit;
        unit.OccupiedTile = this;
    }*/
    public void SetUnit(BaseUnit unit)
    {
        personaje = unit != null ? unit.gameObject : null;

        if (unit != null)
        {
            unit.transform.position = transform.position;
        }
    }
    // Método para establecer el color de un Tile
    public void SetColor(Color color)
    {
        _renderer.color = color;
    }

    // Propiedad para obtener si un Tile está en offset o no
    public bool IsOffset { get { return _isOffset; } }

    public void DestroyOccupiedUnit()
    {
        if (OccupiedUnit != null)
        {
            Destroy(OccupiedUnit.gameObject);
            OccupiedUnit = null;
        }
    }

	//METODO DE TARODEV
	/*public void OnMouseDown() {
        if (this.CompareTag("Basura"))
        {
            DestroyOccupiedUnit();
        }
		//if(GameManager.Instance.GameState != GameState.FightState) return;
	    
		if(OccupiedUnit != null) {
			if(OccupiedUnit.Faction == Faction.Hero) UnitManager.Instance.SetSelectedHero((BaseHero)OccupiedUnit);
			else {
				if (UnitManager.Instance.SelectedHero != null) {
					var enemy = (BaseEnemy) OccupiedUnit;
					Destroy(enemy.gameObject);
					UnitManager.Instance.SetSelectedHero(null);
				}
			}
		}
		else {
			if(UnitManager.Instance.SelectedHero != null) {
				SetUnit(UnitManager.Instance.SelectedHero);
                UnitManager.Instance.SetSelectedHero(null);
            }
		}
	}/*

    // Método que se ejecuta cuando se hace click en un Tile
    /*public void OnMouseDown()
    {
        // Buscamos el objeto GridManager en la escena
        var gridManager = FindObjectOfType<GridManager>();

        // Obtenemos el Tile de inicio para encontrar el camino óptimo
        Tile startTile;
        if (gridManager._lastTile == null)
        {
            startTile = gridManager.GetTileAtPosition(new Vector2(0, 0));
        }
        else
        {
            startTile = gridManager._lastTile;
        }

        // Obtenemos la posición del Tile actual
        var tilePosition = gridManager.GetTileAtPosition(transform.position);

        // Encontramos la ruta óptima entre los Tiles
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
            Debug.Log("No se encontró un camino desde la posición de inicio hasta la posición de destino.");
        }

        // Actualizamos el último Tile seleccionado
        gridManager._lastTile = tilePosition;
    }
*/
    // Método que se ejecuta cuando el mouse entra en un Tile
    public void OnMouseEnter()
    {
        // Activamos el objeto que representa el resaltado
        _highlight.SetActive(true);
        MenuManager.Instance.ShowTileInfo(this);
    }

    // Método que se ejecuta cuando el mouse sale de un Tile
    public void OnMouseExit()
    {
        // Desactivamos el objeto que representa el resaltado
        _highlight.SetActive(false);
        MenuManager.Instance.ShowTileInfo(null);
    }

}