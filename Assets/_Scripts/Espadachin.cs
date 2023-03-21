using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espadachin : MonoBehaviour
{
    private Vector2 tilePos;
    private GridManager gridManager;

    private void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        Tile tilePos = gridManager.GetTileAtPosition(transform.position);
        tilePos.personaje = gameObject;
    }
}
