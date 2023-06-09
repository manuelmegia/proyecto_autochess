using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightManager : MonoBehaviour
{
    public static FightManager Instance;
    private DataService _dataService;
    public Text topScoresText;
    
    public float actionInterval = 1f;
    private float _timer;

    void Awake()
    {
        Instance = this;
        _dataService = new DataService("GameDatabase.db");
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
        /*else if(GameManager.Instance.GameState == GameState.EndState)
        {
            StopCoroutine(PerformActionsCoroutine());
        }*/
    }

    private IEnumerator PerformActionsCoroutine()
    {
        var units = FindObjectsOfType<BaseUnit>();
        var gridManager = FindObjectOfType<GridManager>();

        List<Coroutine> moveCoroutines = new List<Coroutine>();
        List<Coroutine> attackCoroutines = new List<Coroutine>();
    
        foreach (var unit in units)
        {
            if (unit.OccupiedTile.tag != "Bench") {
                if (unit != null && unit.OccupiedTile != null && !unit.IsAttacking)
                {
                    var currentPosition = unit.OccupiedTile.transform.position;
                    var targetUnit = gridManager.FindClosestEnemyUnit(currentPosition, unit.Faction);

                    if (targetUnit != null)
                    {
                       var path = gridManager.FindPath(currentPosition, targetUnit.OccupiedTile.transform.position);

                        // Check if the path exists and the target tile is not occupied
                        if (path != null && path.Count > 1 && !gridManager.IsTileOccupied(path[1]) && !path[1].Reserved)
                        {
                            path[1].Reserved = true;
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
        bool heroesAlive = false;
        bool enemiesAlive = false;

        foreach (var unit in units)
        {
            if (unit.Faction == Faction.Hero)
            {
                if (unit.OccupiedTile.tag != "Bench")
                {
                    heroesAlive = true;
                }
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
            var score = new Score { Round = RoundManager.Instance.GetCurrentRound(), Gold = EconomyManager.Instance.GetCoins() };
            _dataService.CreateScore(score);
            DisplayTopScores();
            UnitManager.Instance.enemyCount = 1;
            GameManager.Instance.ChangeState(GameState.EndState);
        }
        else if (!enemiesAlive)
        {
            Debug.Log("Hero faction has won!");
            RoundManager.Instance.EndRound();
        }
    }
    public void DisplayTopScores()
    {
        var scores = _dataService.GetTopScores();
        string scoreText = "Top 5 Scores: \n";
        foreach (var score in scores)
        {
            scoreText += "Round: " + score.Round + ", Gold: " + score.Gold + ", Time: " + score.Timestamp + "\n";
        }
        topScoresText.text = scoreText;
    }
}