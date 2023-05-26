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
    public bool IsAttacking { get; private set; }
    private BaseUnit selectedUnit;

    
    void Update()
    {
    }

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
    targetTile.Reserved = false;
}
    public void Attack(BaseUnit targetUnit)
    {
        targetUnit.Health -= AttackDamage;

        if (targetUnit.Health <= 0)
        {
            targetUnit.Die();
        }
    }
    public IEnumerator AttackCoroutine(BaseUnit targetUnit, float attackDuration)
    {
        IsAttacking = true;

        // Perform attack animation or delay here, if needed
        yield return new WaitForSeconds(attackDuration);

        // Check if target unit is dead before continuing the attack
        if (targetUnit.IsDead())
        {
            Debug.Log("Target unit " + targetUnit.UnitName + " is dead. Stopping attack.");
            IsAttacking = false;
            yield break;
        }

        // Check if the target unit is within attack range (including diagonals)
        var distance = Vector3.Distance(this.transform.position, targetUnit.transform.position);
        if (distance <= AttackRange)
        {
            targetUnit.Health -= AttackDamage;

            Debug.Log("Target unit " + targetUnit.UnitName + " has " + targetUnit.Health + " health remaining.");

            if (targetUnit.Health <= 0)
            {
                targetUnit.Die();
                IsAttacking = false; // Set IsAttacking to false when the target unit is dead
            }
            else
            {
                StartCoroutine(AttackCoroutine(targetUnit, attackDuration)); // Continue attacking the same target unit
            }
        }
        else
        {
            IsAttacking = false;
            Debug.Log("Target unit " + targetUnit.UnitName + " is out of attack range.");
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
    }
    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(1))
        {
            UIManager.Instance.UpdateStatsText(this);
        }
    }
}