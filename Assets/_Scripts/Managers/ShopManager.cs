using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    public BaseUnit heroPrefab;
    public BaseUnit heroPrefab2;
    private EconomyManager _economyManager;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnHeroButtonClicked()
    {
        if (EconomyManager.Instance.coins >= 1)
        {
            var randomSpawnTile = GridManager.Instance.GetHeroBenchSpawnTile();

            if(randomSpawnTile != null)
            {
                EconomyManager.Instance.DecreaseCoins(1);
                UIManager.Instance.UpdateCoinText(EconomyManager.Instance.coins);
                var spawnedHero = Instantiate(heroPrefab);
                spawnedHero.gameObject.SetActive(true);

                randomSpawnTile.SetUnit(spawnedHero);
                spawnedHero.OccupiedTile = randomSpawnTile; 
                spawnedHero.tag = "BenchHero";
            }
        }
    }
    public void SpawnHero2ButtonClicked()
    {
        if (EconomyManager.Instance.coins >= 1)
        {
            var randomSpawnTile = GridManager.Instance.GetHeroBenchSpawnTile();

            if(randomSpawnTile != null)
            {
                EconomyManager.Instance.DecreaseCoins(3);
                UIManager.Instance.UpdateCoinText(EconomyManager.Instance.coins);
                var spawnedHero = Instantiate(heroPrefab2);
                spawnedHero.gameObject.SetActive(true);

                randomSpawnTile.SetUnit(spawnedHero);
                spawnedHero.OccupiedTile = randomSpawnTile; 
                spawnedHero.tag = "BenchHero";
            }
        }
    }
}