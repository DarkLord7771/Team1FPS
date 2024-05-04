using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    [SerializeField] PlayerController player;

    private void Start()
    {
        player = GetComponent<PlayerController>();
    }
    public void BoughtUpgrade(Upgrade upgrade)  //Determines which upgrade has been purchased
    {
        switch (upgrade.upgradeName)
        {
            case "HP":
                UpgradeHP(upgrade.upgradeValue);
                break;
            case "Speed":
                UpgradeSpeed(upgrade.upgradeValue);
                break;
            case "Jump Distance":
                UpgradeJumpDistance(upgrade.upgradeValue);
                break;
            case "Damage":
                UpgradeDamage(upgrade.upgradeValue);
                break;
            case "Fire Rate":
                UpgradeFireRate(upgrade.upgradeValue);
                break;
            case "Reload Speed":
                UpgradeReloadSpeed(upgrade.upgradeValue);
                break;
            case "Ammo Capacity":
                UpgradeAmmoCapacity(upgrade.upgradeValue);
                break;
            case "Durability":
                UpgradeMeleeDur(upgrade.upgradeValue);
                break;
            default: break;
        }
    }

    void UpgradeHP(float value) //Upgrades player HP
    {
        if (player.HPOrig <= player.maxHP)
        {
            player.HPOrig += (int)value;
            player.HP = player.HPOrig;
            gamemanager.instance.playerUI.UpdateHP();
        }

        if (player.lowHealth)
        {
            player.lowHealth = false;
        }
    }

    void UpgradeSpeed(float value) //Upgrades player Speed
    {
        if (player.speed <= player.maxSpeed)
        {
            player.speedOrig += (int)value;
            player.speed += value;
        }
    }

    void UpgradeJumpDistance(float value) //Upgrades player Jump Distance
    {
        if (player.jumpSpeed <= player.maxJumpSpeed)
        {
            player.jumpSpeedOrig += (int)value;
            player.jumpSpeed += value;
        }
    }

    void UpgradeDamage(float value) //Upgrades player Damage
    {
        player.DamageUpgrade += (int)value;
        player.gunHandler.SelectedGun().shootDamage = player.gunHandler.SelectedGun().baseDamage + player.DamageUpgrade;
    }

    void UpgradeFireRate(float value) //Upgrades player Fire Rate
    {
        player.FireRateUpgrade += value;
        player.gunHandler.SelectedGun().fireRate = player.gunHandler.SelectedGun().baseFireRate + player.FireRateUpgrade;
    }

    void UpgradeReloadSpeed(float value) //Upgrades player Reload Speed
    {
        player.gunHandler.SelectedGun().reloadSpeed += value;
    }

    void UpgradeAmmoCapacity(float value) //Upgrades player ammo capacity
    {
        player.AmmoCapacityUpgrade += (int)value;
        player.gunHandler.SelectedGun().ammoMax = player.gunHandler.SelectedGun().baseMaxAmmo + player.AmmoCapacityUpgrade;
        player.gunHandler.SelectedGun().ammoCur = player.gunHandler.SelectedGun().ammoMax;
        gamemanager.instance.playerUI.UpdateAmmo(player.gunHandler.SelectedGun());
    }

    void UpgradeMeleeDur(float value) //Upgrades player melee durability
    {
        player.meleeHandler.SelectedMelee().durabilityMax += (int)value;
        gamemanager.instance.playerUI.UpdateDur(player.meleeHandler.SelectedMelee());
    }
}
