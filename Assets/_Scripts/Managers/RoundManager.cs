using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;
    private int enemyIncreasePerRound = 1; // Enemies to spawn each round
    public int round;
    void Awake()
    {
        Instance = this;
        round = 0;
    }

    public void EndRound()
    {
        round++;
        // Increase the number of enemies to spawn in UnitManager
        UnitManager.Instance.IncreaseEnemyCount(enemyIncreasePerRound);
        UIManager.Instance.UpdateRoundText(round);
        EconomyManager.Instance.CalculateCoins();
        UnitManager.Instance.SpawnEnemies();
        //GameManager.Instance.ChangeState(GameState.PreparationRound);
    }

    public void StartRound()
    {
        //GameManager.Instance.ChangeState(GameState.FightState);
    }

    public void RepositionUnits()
    {
        // Implement logic to reposition your hero units
        // After repositioning, start the next round
        StartRound();
    }

    public int GetCurrentRound()
    {
        return round;
    }
}

