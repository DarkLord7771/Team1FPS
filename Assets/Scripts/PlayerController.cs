using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] AudioSource aud;

    [Header("----- Player Stats -----")]
    [Range(1, 25)]    [SerializeField] int HP;
    [Range(1, 10)]    [SerializeField] float speed;
    [Range(1, 3)]     [SerializeField] float sprintMod;
    [Range(1, 3)]     [SerializeField] int jumps;
    [Range(5, 25)]    [SerializeField] int jumpSpeed;
    [Range(-15, -35)] [SerializeField] int gravity;

    [SerializeField] int damageUpgrade;
    [SerializeField] int gold;

    [Header("----- Player Max Stats -----")]
    [SerializeField] int maxHP;
    [SerializeField] int maxSpeed;
    [SerializeField] int maxJumpSpeed;

    [Header("----- Gun Stats -----")]
    [SerializeField] List<GunStats> gunList = new List<GunStats>();
    [SerializeField] GameObject gunModel;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float audJumpVol;
    [SerializeField] AudioClip[] audHurt;
    [Range(0, 1)][SerializeField] float audHurtVol;
    [SerializeField] AudioClip[] audSteps;
    [Range(0, 1)][SerializeField] float audStepsVol;

    int jumpCount;
    Vector3 playerVel;
    Vector3 moveDir;
    bool isShooting;
    int HPOrig;
    int selectedGun;
    bool playingSteps;
    bool isSprinting;

    private Vector3 crouchHeight = new Vector3(1, 0.5f, 1);
    private Vector3 playerHeight = new Vector3(1, 1, 1);

    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;

        SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);
        #endif

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
        if(Input.GetKeyDown(KeyCode.LeftShift))
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
            transform.localScale = crouchHeight;
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        }
        else
        {
            transform.localScale = playerHeight;
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }
    }

    IEnumerator Shoot()
    {
        isShooting = true;
        aud.PlayOneShot(gunList[selectedGun].shootSound, gunList[selectedGun].shootSoundVolume);

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>();
            gunList[selectedGun].ammoCur--;
            UpdatePlayerUI();

            if (hit.transform != transform && dmg != null)
            {
                dmg.TakeDamage(shootDamage);
            }
            else
            {
                Instantiate(gunList[selectedGun].hitEffect, hit.point, gunList[selectedGun].hitEffect.transform.rotation);
            }
        }

        //Debug.Log(hit.collider.name);

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
        HP -= amount;
        aud.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);
        StartCoroutine(FlashDamageScreen());
        UpdatePlayerUI();

        if (HP <= 0)
        {
            gamemanager.instance.PlayerHasLost();
        }
    }

    IEnumerator FlashDamageScreen()
    {
        gamemanager.instance.playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gamemanager.instance.playerDamageFlash.SetActive(false);
    }

    public void UpdatePlayerUI()
    {
        gamemanager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
        if (gunList.Count > 0)
        {
            // Update ammo/
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

    public void UpgradeHealth(int amount)
    {
        if (HPOrig != maxHP)
        {
            HPOrig += amount;
            HP = HPOrig;
            UpdatePlayerUI();
        }
    }
    public void UpgradeSpeed(float amount)
    {
        if (speed != maxSpeed)
        {
            speed += amount;
        }
    }

    public void UpgradeJumpSpeed(int amount)
    {
        if (jumpSpeed != maxJumpSpeed)
        {
            jumpSpeed += amount;
        }
    }

    public void UpgradeDamage(int amount)
    {
        // Increment damage upgrade and increase shoot damage.
        damageUpgrade += amount;
        shootDamage = gunList[selectedGun].shootDamage + damageUpgrade;
    }

    public void GetGunStats(GunStats gun)
    {
        gunList.Add(gun);

        shootDamage = gun.shootDamage + damageUpgrade;
        shootDist = gun.shootDist;
        shootRate = gun.shootRate;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.model.GetComponent<MeshRenderer>().sharedMaterial;

        // Set scale of gun model based off of gun's transform.
        gunModel.GetComponent<Transform>().localScale = gun.gunTransform.localScale;

        selectedGun = gunList.Count - 1;
        UpdatePlayerUI();
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
        shootDamage = gunList[selectedGun].shootDamage + damageUpgrade;
        shootDist = gunList[selectedGun].shootDist;
        shootRate = gunList[selectedGun].shootRate;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].model.GetComponent<MeshRenderer>().sharedMaterial;

        //Set scale of gun model based off of gun's transform.
        gunModel.GetComponent<Transform>().localScale = gunList[selectedGun].gunTransform.localScale;

        UpdatePlayerUI();
    }

    public void StopJumping()
    {
        playerVel.y = 0;
        jumpCount = jumps;
    }

    public bool HasMissingAmmo()
    {
        if (gunList.Count > 0)
        {
            for  (int i = 0; i < gunList.Count; i++)
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
        for (int i = 0;i < gunList.Count; i++)
        {
            gunList[i].ammoCur = gunList[i].ammoMax;
        }

        UpdatePlayerUI();
    }
}
