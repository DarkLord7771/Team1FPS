using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    [SerializeField] GunStats gun;
    [SerializeField] float displayTime;
    GameObject buyMenu;
    GameObject notEnoughMenu;
    bool isNearGun;

    // Start is called before the first frame update
    void Start()
    {
        gun.ammoCur = gun.ammoMax;
        buyMenu = gamemanager.instance.menuBuyGun;
        notEnoughMenu = gamemanager.instance.menuNotEnoughGold;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isNearGun)
        {
            gamemanager.instance.SetMenuInactive();

            if (gamemanager.instance.playerScript.BuyGun(gun))
            {
                Destroy(gameObject);
            }
            else
            {
                StartCoroutine(gamemanager.instance.DisplayMessage(notEnoughMenu));
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            gamemanager.instance.SetDisplayMessageActive(buyMenu);
            gamemanager.instance.weaponBuyText.text = "Press E to buy " + gun.gunName;
            gamemanager.instance.weaponCostText.text = "[Cost: " + gun.cost + "]";

            isNearGun = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(gamemanager.instance.menuActive == buyMenu)
        {
            gamemanager.instance.SetMenuInactive();
        }
        
        isNearGun = false;
    }
}
