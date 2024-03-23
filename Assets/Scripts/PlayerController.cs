using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] GameObject bullet;
    [SerializeField] PlayerBullet bulletInfo;

    [Header("----- Player Stats -----")]
    [Range(1, 25)]    [SerializeField] int HP;
    [Range(1, 10)]     [SerializeField] float speed;
    [Range(1, 3)]     [SerializeField] float sprintMod;
    [Range(1, 3)]     [SerializeField] int jumps;
    [Range(5, 25)]    [SerializeField] int jumpSpeed;
    [Range(-15, -35)] [SerializeField] int gravity;
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
    [SerializeField] Transform shootPos;

    int jumpCount;
    Vector3 moveDir;
    Vector3 playerVel;
    bool isShooting;
    int HPOrig;
    int selectedGun;

    private Vector3 crouchHeight = new Vector3(1, 0.5f, 1);
    private Vector3 playerHeight = new Vector3(1, 1, 1);

    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
        UpdatePlayerUI();

        bulletInfo = bullet.GetComponent<PlayerBullet>();
        bulletInfo.SetDamage(shootDamage);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gamemanager.instance.isPaused)
        {
            Sprint();

            Crouch();

            Movement();

            selectGun();

            if (Input.GetButton("Fire1") && !isShooting)
            {
                StartCoroutine(Shoot());
            }
        }
        
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
        }



        playerVel.y += gravity * Time.deltaTime;
        controller.Move(playerVel * Time.deltaTime);

    }

    void Sprint()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed *= sprintMod;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed /= sprintMod;
        }
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

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit))
        {
            CreateBullet(hit.point);

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (hit.transform != transform && dmg != null)
            {
                dmg.TakeDamage(shootDamage);
            }
            else
            {
                Instantiate(gunList[selectedGun].hitEffect, hit.point, gunList[selectedGun].hitEffect.transform.rotation);
            }
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    void CreateBullet(Vector3 target)
    {
        var shotBullet = Instantiate(bullet, shootPos.position, shootPos.transform.rotation);
        shotBullet.GetComponent<Rigidbody>().velocity = (target - shootPos.transform.position).normalized * bulletInfo.speed;
    }

    public void TakeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashDamageScreen());
        UpdatePlayerUI();

        if (HP <= 0)
        {
            gamemanager.instance.playerHasLost();
        }
    }

    IEnumerator flashDamageScreen()
    {
        gamemanager.instance.playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gamemanager.instance.playerDamageFlash.SetActive(false);
    }

    void UpdatePlayerUI()
    {
        gamemanager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
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
        shootDamage += amount;
        bulletInfo.SetDamage(shootDamage);
    }

    public void getGunStats(GunStats gun)
    {
        gunList.Add(gun);

        shootDamage = gun.shootDamage;
        shootDist = gun.shootDist;
        shootRate = gun.shootRate;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.model.GetComponent<MeshRenderer>().sharedMaterial;
    }

    void selectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunList.Count - 1)
        {
            selectedGun++;
            changeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
        {
            selectedGun--;
            changeGun();
        }
    }

    void changeGun()
    {
        shootDamage = gunList[selectedGun].shootDamage;
        shootDist = gunList[selectedGun].shootDist;
        shootRate = gunList[selectedGun].shootRate;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].model.GetComponent<MeshRenderer>().sharedMaterial;
    }
}
