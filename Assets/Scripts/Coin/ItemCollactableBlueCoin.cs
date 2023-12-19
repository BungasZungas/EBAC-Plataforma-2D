using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollactableBlueCoin : ItemCollactableBase
{
    protected override void OnCollect()
    {
        base.OnCollect();
        ItemManager.Instance.AddBlueCoins();
    }
}
