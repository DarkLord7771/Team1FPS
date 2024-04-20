using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShopCustomer
{
    void BoughtUpgrade(Upgrade upgrade);
    bool TrySpendingGold(int upgradeCost);
}
