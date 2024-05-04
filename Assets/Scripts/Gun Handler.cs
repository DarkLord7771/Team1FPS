using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHandler : MonoBehaviour
{
    [SerializeField] PlayerController player;

    [Header("----- Gun Stats -----")]
    [SerializeField] List<GunStats> gunList = new();
    [SerializeField] GameObject gunModel;
    [SerializeField] GunAttack gun;
    [SerializeField] int totalGunsAllowed;

    public bool ChangedGun { get; set; }

    int selectedGun;

    private void Start()
    {
        player = gamemanager.instance.playerScript;
        
    }

    private void Update()
    {
        if (!gamemanager.instance.isPaused)
        {
            SelectGun();

            if (gunList.Count > 0)
            {
                gun.FireWeapon(gunList[selectedGun], gunList.Count);
                gamemanager.instance.playerUI.UpdateAmmo(gunList[selectedGun]);
            }
        }
    }

    public bool BuyGun(GunStats gun) //Buy gun from world
    {
        if (player.Gold >= gun.cost)
        {
            RemoveGun();
            GetGunStats(gun);
            player.Gold -= gun.cost;

            return true;
        }

        return false;
    }

    public void GetGunStats(GunStats gun) //Get gun stats function for when weapon is picked up
    {
        if (gunList.Count > 0)
        {
            ChangedGun = true;
        }

        gunList.Add(gun);

        gunList[selectedGun].shootDamage = gunList[selectedGun].baseDamage + player.DamageUpgrade;
        gunList[selectedGun].fireRate = gunList[selectedGun].baseFireRate + player.FireRateUpgrade;
        gunList[selectedGun].ammoMax = gunList[selectedGun].baseMaxAmmo + player.AmmoCapacityUpgrade;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.model.GetComponent<MeshRenderer>().sharedMaterial;

        // Set scale of gun model based off of gun's transform.
        gunModel.GetComponent<Transform>().localScale = gun.gunTransform.localScale;

        selectedGun = gunList.Count - 1;
        gamemanager.instance.playerUI.DisplayAmmo();
        gamemanager.instance.playerUI.UpdateAmmo(gunList[selectedGun]);
    }

    void ChangeGun() //Changes gun stats when new weapon is selected
    {
        gunList[selectedGun].shootDamage = gunList[selectedGun].baseDamage + player.DamageUpgrade;
        gunList[selectedGun].fireRate = gunList[selectedGun].baseFireRate + player.FireRateUpgrade;
        gunList[selectedGun].ammoMax = gunList[selectedGun].baseMaxAmmo + player.AmmoCapacityUpgrade;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].model.GetComponent<MeshRenderer>().sharedMaterial;

        //Set scale of gun model based off of gun's transform.
        gunModel.GetComponent<Transform>().localScale = gunList[selectedGun].gunTransform.localScale;

        gamemanager.instance.playerUI.UpdateAmmo(gunList[selectedGun]);

        ChangedGun = true;
    }

    void SelectGun() //Gun selection
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunList.Count - 1)
        {
            selectedGun++;
            ChangeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
        {
            selectedGun--;
            ChangeGun();
        }
    }

    public GunStats SelectedGun()
    {
        return gunList[selectedGun];
    }

    public void IncreaseDamage()
    {
        if (player.damagePowerUp && gunList.Count > 0)
        {
            gunList[selectedGun].shootDamage = (int)((gunList[selectedGun].shootDamage + player.DamageUpgrade) * player.DamageMultiplier);
        }
    }

    public void ResetDamage() //Updates weapon damage
    {
        gunList[selectedGun].shootDamage = gunList[selectedGun].baseDamage + player.DamageUpgrade;
    }

    void RemoveGun() //Removes first gun when more than allowed are picked up
    {
        if (gunList.Count >= totalGunsAllowed)
        {
            gunList.RemoveAt(0);
        }
    }

    public void RefillAmmo() //Refills ammo when triggered
    {
        for (int i = 0; i < gunList.Count; i++)
        {
            gunList[i].ammoCur = gunList[i].ammoMax;
        }

        gamemanager.instance.playerUI.UpdateAmmo(gunList[selectedGun]);
    }

    public bool HasGuns() //Checks to make sure player has weapons
    {
        if (gunList.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool HasMissingAmmo() //checks to see if weapons in inventory are empty
    {
        if (gunList.Count > 0)
        {
            for (int i = 0; i < gunList.Count; i++)
            {
                if (gunList[i].ammoCur != gunList[i].ammoMax)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
