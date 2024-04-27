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
    [HideInInspector] public List<GameObject> buttons = new List<GameObject>();

    public GameObject LastSelectedButton {  get; set; }
    public int LastSelectedIndex {  get; set; }

    [SerializeField] int newIndex;



    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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

    private void Update()
    {

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

        if (index == 0)
        {
            EventSystem.current.SetSelectedGameObject(shopButton);
        }
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

    private void HandleNextButtonSelected(int addition)
    {
        if (EventSystem.current.currentSelectedGameObject == null && LastSelectedButton != null)
        {
            newIndex = LastSelectedIndex + addition;
            newIndex = Mathf.Clamp(newIndex, 0, buttons.Count - 1);

            EventSystem.current.SetSelectedGameObject(buttons[newIndex]);
        }
    }
}
