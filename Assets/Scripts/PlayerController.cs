using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;

    [Header("----- Player Stats -----")]
    [Range(1, 10)]    [SerializeField] int HP;
    [Range(1, 5)]     [SerializeField] float speed;
    [Range(1, 3)]     [SerializeField] float sprintMod;
    [Range(1, 3)]     [SerializeField] int jumps;
    [Range(5, 25)]    [SerializeField] int jumpSpeed;
    [Range(-15, -35)] [SerializeField] int gravity;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [SerializeField] Transform shootPos;

    int jumpCount;
    Vector3 moveDir;
    Vector3 playerVel;
    bool isShooting;
    int HPOrig;

    private Vector3 crouchHeight = new Vector3(1, 0.5f, 1);
    private Vector3 playerHeight = new Vector3(1, 1, 1);

    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
        UpdatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {
        Sprint();

        Crouch();

        Movement();

        if (Input.GetButton("Fire1") && !isShooting)
        {
            StartCoroutine(Shoot());
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

        Instantiate(bullet, shootPos.position, shootPos.transform.rotation);

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void TakeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashDamageScreen());
        UpdatePlayerUI();
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

    public void UpgradeHealth(int amount)
    {
        HPOrig += amount;
        HP = HPOrig;
        UpdatePlayerUI();
    }
    public void UpgradeSpeed(float amount)
    {
        speed += amount;
    }

    public void UpgradeJumpSpeed(int amount)
    {
        jumpSpeed += amount;
    }
}
