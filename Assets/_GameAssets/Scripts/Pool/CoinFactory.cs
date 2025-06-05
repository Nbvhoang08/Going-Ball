using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinFactory : MonoBehaviour
{
    public void InitCoins()
    {
        Coin[] coins = GetComponentsInChildren<Coin>();
        foreach (Coin coin in coins)
        {
            coin.ReturnToPool();
        }
        int totalCoin = transform.childCount;
        for (int i = 0; i < totalCoin; i++)
        {
            Transform coinGD = transform.GetChild(i);
            coinGD.gameObject.SetActive(false);

            Coin coin = Pool.Instance.Coin as Coin;
            coin.transform.SetParent(transform);
            coin.transform.localPosition = coinGD.localPosition;
            coin.transform.rotation = Quaternion.identity;
        }
    }

    public void OnResetLevel()
    {
        InitCoins();
    }
}
