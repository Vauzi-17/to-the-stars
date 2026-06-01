using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    public TextMeshProUGUI coinText;
    private int coinCount = 0;

    void Awake()
    {
        instance = this;
    }

    public void AddCoin()
    {
        coinCount++;
        coinText.text = coinCount.ToString();
    }

    public int GetCoin() => coinCount;
}