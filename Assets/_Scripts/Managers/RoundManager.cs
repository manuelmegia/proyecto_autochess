using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;
    private int enemyIncreasePerRound = 1; 
    public int round;
    void Awake()
    {
        Instance = this;
        round = 0;
    }

    public void EndRound()
    {
        round++;
        UnitManager.Instance.IncreaseEnemyCount(enemyIncreasePerRound);
        UIManager.Instance.UpdateRoundText(round);
        UnitManager.Instance.SpawnEnemies();
        GameManager.Instance.ChangeState(GameState.PreparationRound);
    }

    public void StartRound()
    {
        //GameManager.Instance.ChangeState(GameState.FightState);
    }

    public void RepositionUnits()
    {
        UnitManager.Instance.RepositionUnitsFromBench();
        StartRound();
    }

    public int GetCurrentRound()
    {
        return round;
    }
}

