using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Text roundText;
    public Text coinText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void UpdateRoundText(int round)
    {
        roundText.text = "Round: " + round;
    }

    public void UpdateCoinText(int coins)
    {
        coinText.text = "Coins: " + coins;
    }
}