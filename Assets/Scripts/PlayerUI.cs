using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("----- Player UI Elements -----")]
    [SerializeField] Image playerHPBar;
    public PowerUpDisplay PowerUpDisplay;
    [SerializeField] GameObject ammoDisplay;
    [SerializeField] GameObject goldDisplay;
    
    
    [Header("----- UI Text Elements -----")]
    [SerializeField] TMP_Text ammoCurrent;
    [SerializeField] TMP_Text ammoMax;
    [SerializeField] TMP_Text durabilityCur;
    [SerializeField] TMP_Text durabilityMax;
    [SerializeField] TMP_Text goldTotalText;

    [Header("----- Damage Flash Elements -----")]
    [SerializeField] GameObject playerDamageFlash;
    public bool flashActive;

    public void DisplayGold()
    {
        if (!goldDisplay.activeSelf)
        {
            goldDisplay.SetActive(true);
        }
    }

    public void DisplayAmmo()
    {
        if(!ammoDisplay.activeSelf)
        {
            ammoDisplay.SetActive(true);
        }
    }

    public void UpdateGold()
    {
        goldTotalText.text = gamemanager.instance.playerScript.GetGold().ToString("F0");
        ShopManager.instance.UpdateShopGold();
    }

    public void UpdateHP()
    {
        playerHPBar.fillAmount = (float)gamemanager.instance.playerScript.HP / gamemanager.instance.playerScript.HPOrig;
    }

    public void UpdateAmmo(GunStats gun)
    {
        ammoCurrent.text = gun.ammoCur.ToString("F0");
        ammoMax.text = gun.ammoMax.ToString("F0");
    }

    public void UpdateDur(MeleeStats melee)
    {
        durabilityCur.text = melee.durabilityCur.ToString("F0");
        durabilityMax.text = melee.durabilityMax.ToString("F0");
    }

    public IEnumerator FlashDamageScreen() //Flashes effects when damage is taken
    {
        flashActive = true;

        if (playerDamageFlash.GetComponent<Image>().color.a >= .0001f)
            playerDamageFlash.GetComponent<Image>().color = new Color(1, 1, 1, 0);

        for (float i = .5f; i >= 0; i -= Time.deltaTime)
        {
            playerDamageFlash.GetComponent<Image>().color = new Color(1, 1, 1, i);
            yield return new WaitForSeconds(.005f);
        }

        flashActive = false;
    }
}
