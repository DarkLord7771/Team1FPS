using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using static UnityEngine.GraphicsBuffer;
using UnityEditor.Build.Content;
using UnityEngine.UI;
using UnityEngine.Timeline;
using Unity.Burst.CompilerServices;

public class PlayerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] AudioSource aud;
    [SerializeField] PowerUpEffects powerUp;

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
    [SerializeField] int shootRateUpgrade;

    [Header("----- Gun Stats -----")]
    [SerializeField] List<GunStats> gunList = new List<GunStats>();
    [SerializeField] GameObject gunModel;
    public int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;
    [SerializeField] float reloadSpeed;
    [SerializeField] int totalGunsAllowed;

    [Header("----- Laser Weapon -----")]
    [SerializeField] LineRenderer beam;
    [SerializeField] private Transform shootPos;
    [SerializeField] private float beamLength;
    
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

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float audJumpVol;
    [SerializeField] AudioClip[] audHurt;
    [Range(0, 1)][SerializeField] float audHurtVol;
    [SerializeField] AudioClip[] audSteps;
    [Range(0, 1)][SerializeField] float audStepsVol;
    [SerializeField] AudioClip[] shopSounds;

    int jumpCount;
    Vector3 playerVel;
    Vector3 moveDir;
    bool isShooting;
    [HideInInspector] public int HPOrig;
    public bool damagePowerUp;
    int selectedGun;
    bool playingSteps;
    bool isSprinting;
    bool lowHealth;
    bool flashActive;
    bool isInvincible;
    [HideInInspector] public bool hasShield;
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

            if (gunList.Count > 0 && Input.GetButton("Fire1") && !isShooting && gunList[selectedGun].ammoCur > 0)
            {
                StartCoroutine(Shoot());
            }
            else if (gunList.Count > 0 && Input.GetButtonDown("Fire1") && !isShooting && gunList[selectedGun].ammoCur <= 0)
            {
                StartCoroutine(NoAmmoFlash());
            }

            if ((controller.collisionFlags & CollisionFlags.Above) != 0)
            {
                playerVel.y = -1;
            }

            if (lowHealth && !flashActive)
            {
                StartCoroutine(FlashDamageScreen());
            }

            if (damagePowerUp && gunList.Count > 0)
            {
                shootDamage = (int)Mathf.Ceil((gunList[selectedGun].shootDamage + damageUpgrade) * damageMultiplier);
            }
        }
    }

    public void SpawnPlayer()
    {
        HP = HPOrig;
        UpdatePlayerUI();

        controller.enabled = false;
        transform.position = gamemanager.instance.playerStartPos.transform.position;
        controller.enabled = true;
    }

    void Movement()
    {
        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }

        moveDir = Input.GetAxis("Horizontal") * transform.right
                + Input.GetAxis("Vertical") * transform.forward;

        controller.Move(moveDir * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && jumpCount < jumps)
        {
            jumpCount++;
            playerVel.y = jumpSpeed;
            aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);
        }

        playerVel.y += gravity * Time.deltaTime;
        controller.Move(playerVel * Time.deltaTime);

        if (controller.isGrounded && moveDir.normalized.magnitude > 0.3f && !playingSteps)
        {
            StartCoroutine(PlaySteps());
        }
    }

    void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed *= sprintMod;
            isSprinting = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed /= sprintMod;
            isSprinting = false;
        }
    }

    IEnumerator PlaySteps()
    {
        playingSteps = true;

        aud.PlayOneShot(audSteps[Random.Range(0, audSteps.Length)], audStepsVol);

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

    void Crouch()
    {
        if (Input.GetKey(KeyCode.C))
        {
            controller.height = 1;
        }
        else
        {
            controller.height = crouchOrig;
        }
    }

    IEnumerator Shoot()
    {
        isShooting = true;
        aud.PlayOneShot(gunList[selectedGun].shootSound, gunList[selectedGun].shootSoundVolume);

        // Update Ammo Count
        gunList[selectedGun].ammoCur--;
        UpdatePlayerUI();

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (hit.transform != transform && dmg != null && !hit.collider.CompareTag("Player"))
            {
                dmg.TakeDamage(shootDamage);
            }
            else
            {
                Instantiate(gunList[selectedGun].hitEffect, hit.point, gunList[selectedGun].hitEffect.transform.rotation);
                reticle.Expand(reticleRecoil);

            }
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }


    IEnumerator NoAmmoFlash()
    {
        gamemanager.instance.menuNoAmmo.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        gamemanager.instance.menuNoAmmo.SetActive(false);
    }

    public void TakeDamage(int amount)
    {
        if (!isInvincible && !hasShield)
        {
            HP -= amount;
            aud.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);

            if ((float)HP / HPOrig > .3f)
            {
                StartCoroutine(FlashDamageScreen());
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

        UpdatePlayerUI();

        if (HP <= 0)
        {
            gamemanager.instance.PlayerHasLost();
        }
    }

    IEnumerator FlashDamageScreen()
    {
        flashActive = true;

        if (gamemanager.instance.playerDamageFlash.GetComponent<Image>().color.a >= .0001f)
            gamemanager.instance.playerDamageFlash.GetComponent<Image>().color = new Color(1, 1, 1, 0);

        for (float i = .5f; i >= 0; i -= Time.deltaTime)
        {
            gamemanager.instance.playerDamageFlash.GetComponent<Image>().color = new Color(1, 1, 1, i);
            yield return new WaitForSeconds(.005f);
        }

        flashActive = false;
    }

    public void UpdatePlayerUI()
    {
        gamemanager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
        if (gunList.Count > 0)
        {
            if (!gamemanager.instance.ammoDisplay.activeSelf)
            {
                gamemanager.instance.ammoDisplay.SetActive(true);
            }

            // Update ammo
            gamemanager.instance.ammoCurrent.text = gunList[selectedGun].ammoCur.ToString("F0");
            gamemanager.instance.ammoMax.text = gunList[selectedGun].ammoMax.ToString("F0");
        }
    }

    public int GetGold()
    {
        return gold;
    }

    public void SetGold(int amount)
    {
        gold += amount;
    }


    public void GetGunStats(GunStats gun)
    {
        gunList.Add(gun);

        shootDamage = gun.shootDamage;
        shootDist = gun.shootDist;
        shootRate = gun.shootRate;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.model.GetComponent<MeshRenderer>().sharedMaterial;

        // Set scale of gun model based off of gun's transform.
        gunModel.GetComponent<Transform>().localScale = gun.gunTransform.localScale;

        selectedGun = gunList.Count - 1;
        UpdatePlayerUI();
    }

    public void GetMeleeStats(MeleeStats melee)
    {
        meleeList.Add(melee);

        meleeDamage = melee.meleeDamage + damageUpgrade;
        meleeDist = melee.meleeDist;
        meleeRate = melee.meleeRate;
    }

    void SelectGun()
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

    void ChangeGun()
    {
        shootDamage = gunList[selectedGun].shootDamage;
        shootDist = gunList[selectedGun].shootDist;
        shootRate = gunList[selectedGun].shootRate;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].model.GetComponent<MeshRenderer>().sharedMaterial;

        //Set scale of gun model based off of gun's transform.
        gunModel.GetComponent<Transform>().localScale = gunList[selectedGun].gunTransform.localScale;

        UpdatePlayerUI();
    }

    public int GetCurrentDamage()
    {
        return gunList[selectedGun].shootDamage + damageUpgrade;
    }

    public bool HasMissingAmmo()
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

    public bool HasGuns()
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

    public void RefillAmmo()
    {
        for (int i = 0; i < gunList.Count; i++)
        {
            gunList[i].ammoCur = gunList[i].ammoMax;
        }

        UpdatePlayerUI();
    }

    public bool BuyGun(GunStats gun)
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

    void RemoveGun()
    {
        if (gunList.Count >= totalGunsAllowed)
        {
            gunList.RemoveAt(0);
        }
    }

    void UpgradeHP(int value)
    {
        if (HPOrig <= maxHP)
        {
            HPOrig += value;
            HP = HPOrig;
            UpdatePlayerUI();
        }
    }

    void UpgradeSpeed(int value)
    {
        if (speed <= maxSpeed)
        {
            speed += value;
        }
    }

    void UpgradeJumpDistance(int value)
    {
        if (jumpSpeed <= maxJumpSpeed)
        {
            jumpSpeed += value;
        }
    }

    void UpgradeDamage(int value)
    {
        damageUpgrade += value;
        shootDamage += damageUpgrade;
    }

    public void ResetDamage()
    {
        shootDamage = gunList[selectedGun].shootDamage + damageUpgrade;
    }

    public void SetDamageMultiplier(float multiplier)
    {
        damageMultiplier = multiplier;
    }

    public void PlayAudio(AudioClip clip, float volume)
    {
        aud.PlayOneShot(clip, volume);
    }

    public void BoughtUpgrade(Upgrade upgrade)
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
                gunList[selectedGun].shootRate += upgrade.upgradeValue;
                shootRate = gunList[selectedGun].shootRate;
                break;
            case "Reload Speed":
                reloadSpeed += upgrade.upgradeValue;
                break;
            case "Ammo Capacity":
                gunList[selectedGun].ammoMax += upgrade.upgradeValue;
                break;
            default: break;
        }
    }

    public bool TrySpendingGold(int upgradeCost)
    {
        if (gold >= upgradeCost)
        {
            gold -= upgradeCost;
            gamemanager.instance.UpdateGoldDisplay();
            aud.PlayOneShot(shopSounds[0]);
            return true;
        }
        else
        {
            aud.PlayOneShot(shopSounds[1]);
            return false;
        }
    }

    public bool IsNotLaserWeapon()
    {
        if (!gunList[selectedGun].isLaserWeapon)
        {
            return true;
        }
        return false;
    }

    public void SetInvincible()
    {
        isInvincible = !isInvincible;
    }

    public void SetShield()
    {
        hasShield = !hasShield;
    }

    public void BeginPowerUp(PowerUpEffects power)
    {
        powerUp = power;
        aud.PlayOneShot(power.audPowerUp, power.audPowerUpVol);
        gamemanager.instance.PowerUpDisplay.activePowerUps.Add(powerUp);
        StartCoroutine(powerUp.ApplyEffect());
    }


}
