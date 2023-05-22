using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    public static FightManager Instance;

    public float actionInterval = 1f;
    private float _timer;

    void Awake()
    {
        Instance = this;
    }
    /*public void StartFight()
    {
        StartCoroutine(PerformActionsCoroutine());
    }*/
    void Update()
    {
        if (GameManager.Instance.GameState == GameState.FightState)
        {
            _timer += Time.deltaTime;

            if (_timer >= actionInterval)
            {
                _timer = 0;
                StartCoroutine(PerformActionsCoroutine());
            }
        }
    }

    private IEnumerator PerformActionsCoroutine()
    {
        var units = FindObjectsOfType<BaseUnit>();
        var gridManager = FindObjectOfType<GridManager>();

        List<Coroutine> moveCoroutines = new List<Coroutine>();
        List<Coroutine> attackCoroutines = new List<Coroutine>();
    
        foreach (var unit in units)
        {
            if (unit != null && unit.OccupiedTile != null && !unit.IsAttacking)
            {
                var currentPosition = unit.OccupiedTile.transform.position;
                var targetUnit = gridManager.FindClosestEnemyUnit(currentPosition, unit.Faction);

                if (targetUnit != null)
                {
                    var path = gridManager.FindPath(currentPosition, targetUnit.OccupiedTile.transform.position);

                    // Check if the path exists and the target tile is not occupied
                    if (path != null && path.Count > 2 && !gridManager.IsTileOccupied(path[1]))
                    {
                        Coroutine moveCoroutine = StartCoroutine(unit.MoveUnitToTile(path[1], 0.5f));
                        moveCoroutines.Add(moveCoroutine);
                    }
                    else if (path != null && path.Count <= 2 && Vector3.Distance(currentPosition, targetUnit.OccupiedTile.transform.position) <= unit.AttackRange && !unit.IsAttacking)
                    {
                        Coroutine attackCoroutine = StartCoroutine(unit.AttackCoroutine(targetUnit, 0.5f));
                        attackCoroutines.Add(attackCoroutine);
                    }
                }
            }
        }

        // Wait for all attack coroutines to finish
        foreach (var attackCoroutine in attackCoroutines)
        {
            yield return attackCoroutine;
        }

        // Wait for all move coroutines to finish
        foreach (var moveCoroutine in moveCoroutines)
        {
            yield return moveCoroutine;
        }

        CheckWinCondition();
    }



    private void CheckWinCondition()
    {
        var units = FindObjectsOfType<BaseUnit>();
        Faction? winningFaction = null;

        foreach (var unit in units)
        {
            if (unit.IsDead()) continue; // Skip dead units

            if (winningFaction == null)
            {
                winningFaction = unit.Faction; // Set the initial winning faction
            }
            else if (winningFaction != unit.Faction)
            {
                return; // If there are units from multiple factions alive, no one has won yet
            }
        }

        if (winningFaction != null)
        {
            Debug.Log(winningFaction + " faction has won!");
            //UnitManager.Instance.SpawnHeroes();
            RoundManager.Instance.EndRound();
        }
    }
}