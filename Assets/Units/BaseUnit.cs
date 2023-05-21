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
    
    public IEnumerator MoveUnitToTile(Tile targetTile, float duration)
{
    if (OccupiedTile != null)
    {
        OccupiedTile.SetUnit(null);
    }

    Vector3 startPosition = transform.position;
    Vector3 targetPosition = targetTile.transform.position;
    float startTime = Time.time;

    while (Time.time < startTime + duration)
    {
        float t = (Time.time - startTime) / duration;
        transform.position = Vector3.Lerp(startPosition, targetPosition, t);
        yield return null;
    }

    transform.position = targetPosition;
    OccupiedTile = targetTile;
    targetTile.SetUnit(this);
}
    public void Attack(BaseUnit targetUnit)
    {
        targetUnit.Health -= AttackDamage;

        if (targetUnit.Health <= 0)
        {
            targetUnit.Die();
        }
    }
    public void Die()
    {
        if (OccupiedTile != null)
        {
            OccupiedTile.SetUnit(null);
        }

        Destroy(gameObject);
    }
    public bool IsDead()
    {
        return Health <= 0;
        // Replace this with your own logic to determine if the unit is dead
    }
}