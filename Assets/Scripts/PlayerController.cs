using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] PowerUpEffects powerUp;
    [SerializeField] PlayerUI playerUI;

    [Header("----- Player Stats -----")]
    [Range(0, 25)] public int HP;
    public float speed;
    [Range(1, 3)][SerializeField] float sprintMod;
    [Range(1, 3)][SerializeField] int jumps;
    public float jumpSpeed;
    [Range(-15, -35)][SerializeField] int gravity;
    [SerializeField] int gold;

    [Header("----- Player Max Stats -----")]
    [SerializeField] int maxHP;
    [SerializeField] int maxSpeed;
    [SerializeField] int maxJumpSpeed;

    [Header("----- Player Stat Upgrades -----")]
    public int damageUpgrade;
    [SerializeField] float shootRateUpgrade;
    [SerializeField] int ammoCapacityUpgrade;

    [Header("----- Gun Stats -----")]
    [SerializeField] List<GunStats> gunList = new List<GunStats>();
    [SerializeField] GameObject gunModel;
    public int shootDamage;
    [SerializeField] float shootDist;
    [SerializeField] float shootRate;
    [SerializeField] float reloadSpeed;
    [SerializeField] int ammoCapacity;
    [SerializeField] int totalGunsAllowed;

    [Header("----- Laser Weapon -----")]
    [SerializeField] GunAttack gun;
    
    [Header("----- Reticle -----")]
    public Reticle reticle;
    public float reticleRecoil;
    public float settleSpeed;

    [Header("----- Melee Stats -----")]
    [SerializeField] List<MeleeStats> meleeList = new List<MeleeStats>();
    [SerializeField] GameObject meleeModel;
    [SerializeField] int meleeDamage;
    [SerializeField] int meleeDist;
    [SerializeField] float meleeRate;
    [SerializeField] int totalmeleesAllowed;

    [HideInInspector] public int HPOrig;
    [HideInInspector] public bool damagePowerUp;
    [HideInInspector] public bool hasShield;
    [HideInInspector] public bool lowHealth;
    int jumpCount;
    Vector3 playerVel;
    Vector3 moveDir;
    int selectedGun;
    bool playingSteps;
    bool isSprinting;
    bool isInvincible;

    float damageMultiplier;

    float crouchOrig;

    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
        crouchOrig = controller.height;

        SpawnPlayer();

        reticle.SetReturnToCenterSpeed(settleSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gamemanager.instance.isPaused)
        {
            Sprint();

            Crouch();

            Movement();

            SelectGun();

            if (gunList.Count > 0)
            {
                gun.FireWeapon(AudioManager.instance.aud, gunList[selectedGun], gunList.Count);
                playerUI.UpdateAmmo(gunList[selectedGun]);
            }

            if ((controller.collisionFlags & CollisionFlags.Above) != 0)
            {
                playerVel.y = -1;
            }

            if (lowHealth && !playerUI.flashActive)
            {
                StartCoroutine(playerUI.FlashDamageScreen());
            }

            if (damagePowerUp && gunList.Count > 0)
            {
                shootDamage = (int)Mathf.Ceil((gunList[selectedGun].shootDamage + damageUpgrade) * damageMultiplier);
            }
        }
    }

    public void SpawnPlayer() //Spawns player 
    {
        playerUI = gamemanager.instance.playerUI;
        HP = HPOrig;
        playerUI.UpdateHP();

        controller.enabled = false;
        transform.position = gamemanager.instance.playerStartPos.transform.position;
        controller.enabled = true;
    }

    void Movement() //Movement controller
    {
        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }

        moveDir = InputManager.instance.moveDirection.x * transform.right
                + InputManager.instance.moveDirection.y * transform.forward;

        controller.Move(moveDir * speed * Time.deltaTime);

        if (InputManager.instance.jump.action.WasPressedThisFrame() && jumpCount < jumps && gamemanager.instance.menuActive == null)
        {
            jumpCount++;
            playerVel.y = jumpSpeed;
            AudioManager.instance.PlayJumpSound();
        }

        playerVel.y += gravity * Time.deltaTime;
        controller.Move(playerVel * Time.deltaTime);

        if (controller.isGrounded && moveDir.normalized.magnitude > 0.3f && !playingSteps)
        {
            StartCoroutine(PlaySteps());
        }
    }

    void Sprint() //Sprint action
    {
        if (InputManager.instance.SprintPressedInput)
        {
            speed *= sprintMod;
            isSprinting = true;
        }
        else if (InputManager.instance.SprintReleasedInput)
        {
            speed /= sprintMod;
            isSprinting = false;
        }
    }

    IEnumerator PlaySteps() //Plays step sounds to audio
    {
        playingSteps = true;

        AudioManager.instance.PlayFootSteps();

        if (!isSprinting)
        {
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
        }

        playingSteps = false;
    }

    void Crouch() //Crouch action
    {
        if (InputManager.instance.crouch.action.IsPressed())
        {
            controller.height = 1;
        }
        else
        {
            controller.height = crouchOrig;
        }
    }

    IEnumerator NoAmmoFlash() //Flashes when player has no ammo
    {
        gamemanager.instance.menuNoAmmo.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        gamemanager.instance.menuNoAmmo.SetActive(false);
    }

    public void TakeDamage(int amount) //Take damage function
    {
        if (!isInvincible && !hasShield)
        {
            HP -= amount;
            AudioManager.instance.PlayHurtSound();

            if ((float)HP / HPOrig > .3f)
            {
                StartCoroutine(playerUI.FlashDamageScreen());
            }
            else
            {
                lowHealth = true;
            }
        }
        else if (hasShield)
        {
            SetShield();
        }

        playerUI.UpdateHP();

        if (HP <= 0)
        {
            gamemanager.instance.PlayerHasLost();
        }
    }

    public int GetGold() //Gold getter
    {
        return gold;
    }

    public void SetGold(int amount) //Gold setter
    {
        gold += amount;
    }

    public void GetGunStats(GunStats gun) //Get gun stats function for when weapon is picked up
    {
        gunList.Add(gun);

        shootDamage = gun.shootDamage;
        shootDist = gun.shootDist;
        shootRate = gun.fireRate;
        ammoCapacity = gun.ammoMax;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.model.GetComponent<MeshRenderer>().sharedMaterial;

        // Set scale of gun model based off of gun's transform.
        gunModel.GetComponent<Transform>().localScale = gun.gunTransform.localScale;

        selectedGun = gunList.Count - 1;
        playerUI.DisplayAmmo();
        playerUI.UpdateAmmo(gunList[selectedGun]);
    }

    public void GetMeleeStats(MeleeStats melee) //Gets melee stats for melee weapon
    {
        meleeList.Add(melee);

        meleeDamage = melee.meleeDamage + damageUpgrade;
        meleeDist = melee.meleeDist;
        meleeRate = melee.meleeRate;
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

    void ChangeGun() //Changes gun stats when new weapon is selected
    {
        shootDamage = gunList[selectedGun].shootDamage;
        shootDist = gunList[selectedGun].shootDist;
        shootRate = gunList[selectedGun].fireRate;
        ammoCapacity = gunList[selectedGun].ammoMax;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].model.GetComponent<MeshRenderer>().sharedMaterial;

        //Set scale of gun model based off of gun's transform.
        gunModel.GetComponent<Transform>().localScale = gunList[selectedGun].gunTransform.localScale;

        playerUI.UpdateAmmo(gunList[selectedGun]);
    }

    public int GetCurrentDamage() //Gives current damage from gun and modifiers
    {
        return (int)gunList[selectedGun].shootDamage + damageUpgrade;
    }

    public bool HasMissingAmmo() //checks to see if weapons in inventory are empty
    {
        if (gunList.Count > 0)
        {
            for (int i = 0; i < gunList.Count; i++)
            {
                if (gunList[i].ammoCur != ammoCapacity)
                {
                    return true;
                }
            }
        }

        return false;
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

    public void RefillAmmo() //Refills ammo when triggered
    {
        for (int i = 0; i < gunList.Count; i++)
        {
            gunList[i].ammoCur = ammoCapacity;
        }

        playerUI.UpdateAmmo(gunList[selectedGun]);
    }

    public bool BuyGun(GunStats gun) //Buy gun from world
    {
        if (gold >= gun.cost)
        {
            RemoveGun();
            GetGunStats(gun);
            gold -= gun.cost;

            return true;
        }

        return false;
    }

    void RemoveGun() //Removes first gun when more than allowed are picked up
    {
        if (gunList.Count >= totalGunsAllowed)
        {
            gunList.RemoveAt(0);
        }
    }

    void UpgradeHP(float value) //Upgrades player HP
    {
        if (HPOrig <= maxHP)
        {
            HPOrig += (int)value;
            HP = HPOrig;
            playerUI.UpdateHP();
        }
        
        if (lowHealth)
        {
            lowHealth = false;
        }
    }

    void UpgradeSpeed(float value) //Upgrades player Speed
    {
        if (speed <= maxSpeed)
        {
            speed += value;
        }
    }

    void UpgradeJumpDistance(float value) //Upgrades player Jump Distance
    {
        if (jumpSpeed <= maxJumpSpeed)
        {
            jumpSpeed += value;
        }
    }

    void UpgradeDamage(float value) //Upgrades player Damage
    {
        damageUpgrade += (int)value;
        shootDamage += damageUpgrade;
    }

    void UpgradeFireRate(float value) //Upgrades player Fire Rate
    {
        shootRate += value;
    }

    void UpgradeReloadSpeed(float value) //Upgrades player Reload Speed
    {
        reloadSpeed += value;
    }

    void UpgradeAmmoCapacity(float value) //Upgrades player ammo capacity
    {
        ammoCapacity += (int)value;
        playerUI.UpdateAmmo(gunList[selectedGun]);
    }

    public void ResetDamage() //Updates weapon damage
    {
        shootDamage = gunList[selectedGun].shootDamage + damageUpgrade;
    }

    public void SetDamageMultiplier(float multiplier) //Sets damage multiplier
    {
        damageMultiplier = multiplier;
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
            default: break;
        }
    }

    public bool TrySpendingGold(int upgradeCost) //Checks to see if gold can be spent
    {
        if (gold >= upgradeCost)
        {
            gold -= upgradeCost;
            playerUI.UpdateGold();
            AudioManager.instance.PlayShopGoodSound();
            return true;
        }
        else
        {
            AudioManager.instance.PlayShopBadSound();
            return false;
        }
    }

    public bool IsNotLaserWeapon()  //Checks to see if weapon is laser weapon
    {
        if (!gunList[selectedGun].isLaserWeapon)
        {
            return true;
        }
        return false;
    }

    public void SetInvincible() //Sets the player temporarily invincible
    {
        isInvincible = !isInvincible;
    }

    public void SetShield() //Give player a shield
    {
        hasShield = !hasShield;
    }

    public void BeginPowerUp(PowerUpEffects power) //Sets active power up on pickup
    {
        powerUp = power;
        AudioManager.instance.PlayPowerUpSound();
        playerUI.PowerUpDisplay.activePowerUps.Add(powerUp);
        StartCoroutine(powerUp.ApplyEffect());
    }
}
