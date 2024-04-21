using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Upgrade
{
    public string upgradeName;
    public int upgradeValue;
    public int cost;
    [HideInInspector] public int purchasedQuantity;
}
