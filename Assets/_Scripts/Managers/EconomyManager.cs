using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;
    public int coins;

    void Awake()
    {
        Instance = this;
        coins = 0;
    }

    public void CalculateCoins()
    {
        int aliveHeroes = FindObjectsOfType<BaseUnit>().Count(unit => unit.Faction == Faction.Hero && !unit.IsDead());
        coins += 1 + aliveHeroes;
        UIManager.Instance.UpdateCoinText(coins);
    }
    public int DecreaseCoins(int dCoins)
    {
        return coins -= dCoins;
    }
    public int GetCoins()
    {
        return coins;
    }
}
