using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{
    Animator anim;
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
    public int Gold { get; set; }

    [Header("----- Player Max Stats -----")]
    public int maxHP;
    public int maxSpeed;
    public int maxJumpSpeed;

    [Header("----- Weapon Management -----")]
    public GunHandler gunHandler;
    public MeleeHandler meleeHandler;
    
    [Header("----- Reticle -----")]
    public Reticle reticle;
    public float reticleRecoil;
    public float settleSpeed;

    [HideInInspector] public int HPOrig;
    [HideInInspector] public bool damagePowerUp;
    [HideInInspector] public bool lowHealth;

    public int DamageUpgrade { get; set; }
    public float FireRateUpgrade { get; set; }
    public int AmmoCapacityUpgrade { get; set; }

    public float DamageMultiplier {  get; set; }
    public bool HasShield {  get; set; }
    public bool IsInvincible {  get; set; }

    int jumpCount;
    Vector3 playerVel;
    Vector3 moveDir;
    int selectedMelee;
    bool playingSteps;
    bool isSprinting;
    

    float crouchOrig;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
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

            if ((controller.collisionFlags & CollisionFlags.Above) != 0)
            {
                playerVel.y = -1;
            }

            if (lowHealth && !playerUI.flashActive)
            {
                playerUI.StartCoroutine(playerUI.FlashDamageScreen());
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

    #region Locomotion Methods
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

        if (InputManager.instance.jump.action.WasPressedThisFrame() && jumpCount < jumps)
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
    #endregion

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

    public void TakeDamage(int amount) //Take damage function
    {
        if (!IsInvincible && !HasShield)
        {
            HP -= amount;
            AudioManager.instance.PlayHurtSound();

            if ((float)HP / HPOrig > .3f)
            {
                playerUI.StartCoroutine(playerUI.FlashDamageScreen());
            }
            else
            {
                lowHealth = true;
            }
        }
        else if (HasShield)
        {
            HasShield = false;
        }

        playerUI.UpdateHP();

        if (HP <= 0)
        {
            gamemanager.instance.PlayerHasLost();
        }
    }

    public bool TrySpendingGold(int upgradeCost) //Checks to see if gold can be spent
    {
        if (Gold >= upgradeCost)
        {
            Gold -= upgradeCost;
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

    public bool HasLaserWeaponEquipped()  //Checks to see if weapon is laser weapon
    {
        if (gunHandler.SelectedGun().isLaserWeapon)
        {
            return true;
        }
        return false;
    }

    public void BeginPowerUp(PowerUpEffects power) //Sets active power up on pickup
    {
        powerUp = power;
        AudioManager.instance.PlayPowerUpSound();
        playerUI.PowerUpDisplay.activePowerUps.Add(powerUp);
        StartCoroutine(powerUp.ApplyEffect());
    }

    public void SetHP()
    {
        HP = HPOrig;
    }

    public void CallHPUIUpdate()
    {
        playerUI.UpdateHP();
    }
}
