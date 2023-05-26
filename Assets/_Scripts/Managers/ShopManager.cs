using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    public BaseUnit heroPrefab;
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
            }
        }
    }
}