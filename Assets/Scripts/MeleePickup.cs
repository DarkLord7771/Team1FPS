using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleePickup : MonoBehaviour
{
    [SerializeField] MeleeStats melee;
    [SerializeField] float displayTime;
    GameObject buyMenu;
    GameObject notEnoughMenu;
    bool isNearMelee;

    // Start is called before the first frame update
    void Start()
    {
        SetBaseMeleeStats();

        buyMenu = gamemanager.instance.menuBuyMelee;
        notEnoughMenu = gamemanager.instance.menuNotEnoughGold;
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.instance.interact.action.WasPressedThisFrame() && isNearMelee)
        {
            gamemanager.instance.SetMenuInactive();

            if (gamemanager.instance.playerScript.meleeHandler.BuyMelee(melee))
            {
                Destroy(gameObject);
            }
            else
            {
                StartCoroutine(gamemanager.instance.DisplayMessage(notEnoughMenu));
            }
        }
    }

    void SetBaseMeleeStats()
    {
        melee.meleeDamage = melee.baseMeleeDamage;
        melee.meleeDist = melee.baseMeleeDist;
        melee.meleeSpeed = melee.baseMeleeSpeed;
        melee.durabilityCur = melee.durabilityMax;
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            gamemanager.instance.SetDisplayMessageActive(buyMenu);
            gamemanager.instance.weaponBuyText.text = "Press E to buy " + melee.meleeName;
            gamemanager.instance.weaponCostText.text = "[Cost: " + melee.cost + "]";

            isNearMelee = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (gamemanager.instance.menuActive == buyMenu)
        {
            gamemanager.instance.SetMenuInactive();
        }

        isNearMelee = false;
    }
}
