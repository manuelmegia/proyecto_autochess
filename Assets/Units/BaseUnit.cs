using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BaseUnit : MonoBehaviour
{
    public string UnitName;
    public Tile OccupiedTile;
    public Faction Faction;
    public int Health;
    public int AttackDamage;
    public float AttackRange;
    
    public void MoveToTile(Tile targetTile)
    {
        if (OccupiedTile != null)
        {
            OccupiedTile.SetUnit(null);
        }

        OccupiedTile = targetTile;
        targetTile.SetUnit(this);
        transform.position = targetTile.transform.position;
    }

}