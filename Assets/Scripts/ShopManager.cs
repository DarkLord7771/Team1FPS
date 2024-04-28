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
    public List<GameObject> buttons = new List<GameObject>();

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
        for (int i = 0; i < upgrades.Length; i++)
        {
            CreateButton(upgrades[i], i);
        }
    }

    private void CreateButton(Upgrade upgrade, int index)
    {
        GameObject shopButton = Instantiate(shopPrefab, shopContentWindow);
        Transform shopButtonTransform = shopButton.transform;

        UpdateButtonText(shopButtonTransform, upgrade);

        shopButtonTransform.GetComponent<Button>().onClick.AddListener(() =>
        {
            TryBuyUpgrade(upgrade);
            UpdateButtonText(shopButtonTransform, upgrade);
        });

        buttons.Add(shopButton);
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
            gamemanager.instance.playerUpgrade.BoughtUpgrade(upgrade);

            upgrade.cost *= upgradeCostMultiplier; 
        }
    }

    public void UpdateShopGold()
    {
        shopGoldDisplay.text = gamemanager.instance.playerScript.Gold.ToString("F0");
    }
}
