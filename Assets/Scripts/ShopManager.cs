using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    [Header("----- Components -----")]
    [SerializeField] Transform shopContentWindow;
    [SerializeField] GameObject shopPrefab;
    [SerializeField] TMP_Text shopGoldDisplay;

    [Header("----- Upgrades -----")]
    public Upgrade[] upgrades;
    [SerializeField] int upgradeCostMultiplier;
    

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach (Upgrade upgrade in upgrades)
        {
            CreateButton(upgrade);
        }
    }

    private void CreateButton(Upgrade upgrade)
    {
        GameObject shopButton = Instantiate(shopPrefab, shopContentWindow);
        Transform shopButtonTransform = shopButton.transform;

        upgrade.itemRef = shopButton;

        UpdateButtonText(shopButtonTransform, upgrade);


        shopButtonTransform.GetComponent<Button>().onClick.AddListener(() =>
        {
            TryBuyUpgrade(upgrade);
            UpdateButtonText(shopButtonTransform, upgrade);
        });
    }

    private void UpdateButtonText(Transform buttonTransform, Upgrade upgrade)
    {
        buttonTransform.Find("Upgrade").GetComponent<TMP_Text>().text = "Increase " + upgrade.upgradeName;
        buttonTransform.Find("Cost").GetComponent<TMP_Text>().text = upgrade.cost.ToString("F0") + " G";
    }

    private void TryBuyUpgrade(Upgrade upgrade)
    {
        if (gamemanager.instance.playerScript.TrySpendingGold(upgrade.cost))
        {
            gamemanager.instance.playerScript.BoughtUpgrade(upgrade);

            upgrade.cost *= upgradeCostMultiplier; 
        }
    }

    public void UpdateShopGold()
    {
        shopGoldDisplay.text = gamemanager.instance.playerScript.GetGold().ToString("F0");
    }
}
