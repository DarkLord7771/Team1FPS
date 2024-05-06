using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Upgrade
{
    public string upgradeName;
    public float upgradeValue;
    public float maxUpgradeValue;
    public float currentUpgradeValue;
    public int cost;
    [HideInInspector] public int purchasedQuantity;
}
