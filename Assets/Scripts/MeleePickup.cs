using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleePickup : MonoBehaviour
{
    [SerializeField] GunStats melee;
    [SerializeField] float displayTime;
    GameObject buyMenu;
    GameObject notEnoughMenu;
    bool isNearMelee;
    // Start is called before the first frame update
    void Start()
    {
        melee.ammoCur = melee.ammoMax;
        buyMenu = gamemanager.instance.menuBuyMelee;
        notEnoughMenu = gamemanager.instance.menuNotEnoughGold;
        isNearMelee = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isNearMelee)
        {
            gamemanager.instance.SetMenuInactive();

            if (gamemanager.instance.playerScript.BuyGun(melee))
            {
                Destroy(gameObject);
            }
            else
            {
                StartCoroutine(gamemanager.instance.DisplayMessage(notEnoughMenu));
            }
        }
    }
}
