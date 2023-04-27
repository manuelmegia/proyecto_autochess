using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
	public static UnitManager Instance;
	private List<ScriptableUnit> _units;

    void Awake()
    {
        Instance = this;
		units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
    }

    public void SpawnHeroes()
	{
		var heroCount = 1;
		for (int i = 0; i < heroCount; i++){
			var randomPrefab = GetTandomUnit<BaseHero>(Faction.Hero);
			var spawnedHero = Instantiate(randomPrefab);
			var randomSpawnTile = GridManager.Instance.GetHeroSpawnTile();
			
			randomSpawnTile.SetUnit(spawnedHero);
		}
		GameManager.Instance.ChangeState(GameState.SpawnEnemies);
	}

	public void SpawnEnemies()
	{
		var enemyCount = 1;
		for (int i = 0; i < enemyCount; i++){
			var randomPrefab = GetTandomUnit<BaseEnemy>(Faction.Enemy);
			var spawnedEnemy = Instantiate(randomPrefab);
			var randomSpawnTile = GridManager.Instance.GetEnemySpawnTile();
			
			randomSpawnTile.SetUnit(spawnedEnemy);
		}
		GameManager.Instance.ChangeState(GameState.FightState);
	}
	private T gerRandomUnit<T>(Faction faction) where T : BaseUnit {
		return (T)_units.Where(u->u.Faction).OrderBy(o->Random.value).First().UnitPrefab;
	}
}
