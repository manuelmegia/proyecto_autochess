using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsDisplay : MonoBehaviour
{
    public static StatsDisplay Instance;

    [SerializeField]
    private Text statsText;

    void Awake()
    {
        Instance = this;
    }

    public void ShowStats(BaseUnit unit)
    {
        statsText.text = $"Name: {unit.UnitName}\nHealth: {unit.Health}\nAttack Damage: {unit.AttackDamage}\nAttack Range: {unit.AttackRange}";
    }
}
