using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    public float actionInterval = 1f;
    private float _timer;

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= actionInterval)
        {
            _timer = 0;
            PerformActions();
        }
    }

    private void PerformActions()
    {
        var units = FindObjectsOfType<BaseUnit>();
        var gridManager = FindObjectOfType<GridManager>();

        foreach (var unit in units)
        {
            var currentPosition = unit.OccupiedTile.transform.position;
            var targetTile = gridManager.FindClosestEnemyTile(currentPosition, unit.Faction);

            if (targetTile != null)
            {
                var path = gridManager.FindPath(currentPosition, targetTile.transform.position);

                if (path != null && path.Count > 1)
                {
                    unit.MoveToTile(path[1]);
                }
                else if (path != null && path.Count == 1)
                {
                    // Attack logic here
                }
            }
        }
    }
}