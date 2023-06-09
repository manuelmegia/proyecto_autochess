using UnityEngine;

public class SelectableUnit : MonoBehaviour
{
    private BaseUnit _baseUnit;
    private bool _isSelected;

    void Awake()
    {
        _baseUnit = GetComponent<BaseUnit>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                var unit = hit.collider.GetComponent<BaseUnit>();
                if (unit != null && unit == _baseUnit && GameManager.Instance.GameState == GameState.PreparationRound)
                {
                    _isSelected = true;
                    Debug.Log("click en heroe");
                }
                else if (unit == null && _isSelected)
                {
                    var tile = hit.collider.GetComponent<Tile>();
                    if (tile != null && !tile.OccupiedUnit)
                    {
                        _baseUnit.MoveToTile(tile);
                        _isSelected = false;
                    }
                }
            }
        }
    }
}