using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject heroPrefab;
    public Transform benchArea; // The area where new heroes are placed

    public void PurchaseHero()
    {
        if (EconomyManager.Instance.GetCoins() >= 1)
        {
            EconomyManager.Instance.DecreaseCoins(1);
            UnitManager.Instance.SpawnBenchHeroes();
        }
        else
        {
            Debug.Log("Not enough coins to purchase hero.");
        }
    }
}