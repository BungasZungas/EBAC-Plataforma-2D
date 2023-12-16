using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZUNGAS.Core.Singleton;
using TMPro;

public class ItemManager : Singleton<ItemManager>
{
    public int coins;

    public TMP_Text coinsText;

    private void Start()
    {
        Reset();
    }

    private void Update()
    {
        DrawCoins();
    }

    private void Reset()
    {
        coins = 0;
    }

    public void AddCoins(int amount = 1)
    {
        coins += amount;
    }

    public void DrawCoins()
    {
        coinsText.text = "x " + coins.ToString();
    }
}
