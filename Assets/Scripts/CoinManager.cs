using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField]Coin[] coins;

    private void OnDisable()
    {
        GameManager.Instance.OnGameReset -= ResetCoins;
    }

    private void Awake()
    {
        if (coins.Length == 0)
        {
            GetCoins();
        }
    }

    private void Start()
    {
        GameManager.Instance.OnGameReset += ResetCoins;
    }

    void ResetCoins()
    {
        for (int i = 0; i < coins.Length; i++)
        {
            var coin = coins[i];

            coin.gameObject.SetActive(true);
        }
    }

    void GetCoins()
    {
        coins = GetComponentsInChildren<Coin>();
    }

    private void OnValidate()
    {
        GetCoins();
    }
}