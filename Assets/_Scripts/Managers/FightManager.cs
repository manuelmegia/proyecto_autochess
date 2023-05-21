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
            StartCoroutine(PerformActionsCoroutine());
        }
    }

    private IEnumerator PerformActionsCoroutine()
    {
        var units = FindObjectsOfType<BaseUnit>();
        var gridManager = FindObjectOfType<GridManager>();

        List<Coroutine> moveCoroutines = new List<Coroutine>();

        foreach (var unit in units)
        {
            if (unit != null && unit.OccupiedTile != null)
            {
                // Add debug logs to check if hero units are being processed
                if (unit.Faction == Faction.Hero)
                {
                    Debug.Log("Processing hero unit: " + unit.name);
                }

                var currentPosition = unit.OccupiedTile.transform.position;
                var targetTile = gridManager.FindClosestEnemyTile(currentPosition, unit.Faction);

                if (targetTile != null)
                {
                    var path = gridManager.FindPath(currentPosition, targetTile.transform.position);

                    if (path != null && path.Count > 1)
                    {
                        Coroutine moveCoroutine = StartCoroutine(unit.MoveUnitToTile(path[1], 0.5f));
                        moveCoroutines.Add(moveCoroutine);
                    }
                    else if (path != null && path.Count == 1)
                    {
                        // Attack logic
                        var targetUnit = targetTile.personaje.GetComponent<BaseUnit>();
                        if (targetUnit != null)
                        {
                            unit.Attack(targetUnit);
                        }
                    }
                }
            }

            // Wait for all move coroutines to finish
            foreach (var moveCoroutine in moveCoroutines)
            {
                yield return moveCoroutine;
            }
        }
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        var units = FindObjectsOfType<BaseUnit>();
        bool heroesAlive = false;
        bool enemiesAlive = false;

        foreach (var unit in units)
        {
            if (unit.Faction == Faction.Hero)
            {
                heroesAlive = true;
            }
            else if (unit.Faction == Faction.Enemy)
            {
                enemiesAlive = true;
            }

            if (heroesAlive && enemiesAlive)
            {
                break;
            }
        }

        if (!heroesAlive)
        {
            Debug.Log("Enemy faction has won!");
        }
        else if (!enemiesAlive)
        {
            Debug.Log("Hero faction has won!");
        }
    }
}