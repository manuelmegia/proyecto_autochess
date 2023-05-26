using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
	public static UnitManager Instance;
	private List<ScriptableUnit> _units;

	public BaseHero SelectedHero;
	public int enemyCount;
	
	public void IncreaseEnemyCount(int amount)
	{
		enemyCount += amount;
	}
    void Awake()
    {
        Instance = this;
		_units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
		Debug.Log("List of components in _units:");
		foreach (var unit in _units)
		{
			Debug.Log($"Unit Name: {unit.UnitPrefab.UnitName}, Faction: {unit.Faction}");
		}
		enemyCount = 1;
    }

    public void SpawnHeroes()
	{
		var heroCount = 4;
		for (int i = 0; i < heroCount; i++){
			var randomPrefab = GetRandomUnit<BaseHero>(Faction.Hero);
			var spawnedHero = Instantiate(randomPrefab);
			var randomSpawnTile = GridManager.Instance.GetHeroSpawnTile();
			
			randomSpawnTile.SetUnit(spawnedHero);
			
			spawnedHero.OccupiedTile = randomSpawnTile;
		}
		GameManager.Instance.ChangeState(GameState.SpawnEnemies);
	}

	public void SpawnEnemies()
	{
		for (int i = 0; i < enemyCount; i++){
			var randomPrefab = GetRandomUnit<BaseEnemy>(Faction.Enemy);
			var spawnedEnemy = Instantiate(randomPrefab);
			var randomSpawnTile = GridManager.Instance.GetEnemySpawnTile();
			
			spawnedEnemy.OccupiedTile = randomSpawnTile;
			randomSpawnTile.SetUnit(spawnedEnemy);
		}
		GameManager.Instance.ChangeState(GameState.FightState);
	}
	private T GetRandomUnit<T>(Faction faction) where T : BaseUnit {
		return (T)_units.Where(u => u.Faction == faction).OrderBy(o => Random.value).First().UnitPrefab;
	}

	public void SetSelectedHero(BaseHero hero)
	{
		SelectedHero = hero;
		MenuManager.Instance.ShowSelectedHero(hero);
	}
}
