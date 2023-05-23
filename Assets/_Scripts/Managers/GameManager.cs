using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState GameState;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ChangeState(GameState.GenerateGrid);
    }

    public void ChangeState(GameState newState)
    {
        GameState = newState;
        switch (newState)
        {
            case GameState.GenerateGrid:
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.SpawnHeroes:
                UnitManager.Instance.SpawnHeroes();
                break;
            case GameState.SpawnEnemies:
                UnitManager.Instance.SpawnEnemies();
                break;
            case GameState.PreparationRound:
                EconomyManager.Instance.CalculateCoins();
                RoundManager.Instance.RepositionUnits();
                break;
            case GameState.FightState:
                //FightManager.Instance.StartFight();
                break;
            case GameState.EndState:
                var unit= FindObjectsOfType<BaseUnit>();
                foreach (BaseUnit baseUnit in unit)
                {
                    baseUnit.Die();
                }

                // Reset rounds and gold
                RoundManager.Instance.round = 0;
                EconomyManager.Instance.coins = 0;
                // Call the SpawnHeroes method
                UnitManager.Instance.SpawnHeroes();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
}

public enum GameState
{
    GenerateGrid = 0,
    SpawnHeroes = 1,
    SpawnEnemies = 2,
    PreparationRound = 3,
    FightState = 4,
    EndState = 5
}