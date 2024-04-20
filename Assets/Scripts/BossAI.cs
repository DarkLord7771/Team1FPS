using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent bossAgent;
    [SerializeField] Animator anim;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] GameObject Bullet;
    [SerializeField] AudioSource aud;
    [SerializeField] Slider healthbar;
    [SerializeField] GameObject Explosion;
    public WaveSpawner whereISpawned;

    [Header("----- Enemy Stats -----")]
    [SerializeField] int HP;
    int maxHP;
    [SerializeField] float shootRate;
    [SerializeField] int gold;

    [Header("----- Enemy Locomotion -----")]
    [SerializeField] int faceTargetSpeed;
    [SerializeField] float animSpeedTrans;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audBossHurt;
    [Range(0, 1)][SerializeField] float audBossHurtVol;
    [SerializeField] AudioClip[] audBossSteps;
    [Range(0, 1)][SerializeField] float audBossStepsVol;
    [SerializeField] AudioClip[] audEnemyShoot;
    [Range(0, 1)][SerializeField] float audBossShootVol;

    bool isShooting;
    Vector3 playerDir;
    WeaponIk weaponIk;

    // Start is called before the first frame update
    void Start()
    {
        maxHP = HP;

        // Find healthbar and set it to not active on start.
        healthbar = GameObject.Find("Health Bar").GetComponent<Slider>();
        healthbar.gameObject.SetActive(false);

        weaponIk = GetComponent<WeaponIk>();
        weaponIk.SetAimTransform(shootPos);
    }

    // Update is called once per frame
    void Update()
    {
        // Get magnitude of normalized agent velocity.
        float animSpeed = bossAgent.velocity.normalized.magnitude;

        //Set animator Speed float to lerp to velocity based off of animSpeedTrans.
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), animSpeed, animSpeedTrans * Time.deltaTime));

        healthbar.transform.rotation = Camera.main.transform.rotation;
        PursuePlayer();
    }

    void PursuePlayer()
    {
        bossAgent.SetDestination(gamemanager.instance.player.transform.position);

        // Get players direction.
        playerDir = gamemanager.instance.player.transform.position - headPos.position;
        weaponIk.SetTargetTransform(gamemanager.instance.player.transform);

        // Get raycast hit and from head position to player direction and store it in hit.
        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit, 15))
        {
            // If collider is the player start shooting.
            if (hit.collider.CompareTag("Player"))
            {
                if (!isShooting)
                {
                    StartCoroutine(Shoot());
                }

                // If remaining distance is less than or equal to stopping distance.
                if (bossAgent.remainingDistance <= bossAgent.stoppingDistance)
                {
                    FaceTarget();
                }
            }
        }
    }

    void FaceTarget()
    {
        // Slowly rotate enemy toward player.
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    IEnumerator Shoot()
    {
        isShooting = true;
        anim.SetTrigger("Firing");
        aud.PlayOneShot(audEnemyShoot[Random.Range(0, audEnemyShoot.Length)], audBossShootVol);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void CreateBullet()
    {
        Instantiate(Bullet, shootPos.position, shootPos.rotation);
    }

    public void TakeDamage(int amount)
    {
        HP -= amount;
        anim.SetTrigger("Damage");
        StartCoroutine(FlashRed());
        aud.PlayOneShot(audBossHurt[Random.Range(0, audBossHurt.Length)], audBossHurtVol);
        SetHealthBar();

        if (HP <= 0)
        {
            gamemanager.instance.playerScript.SetGold(gold);
            gamemanager.instance.UpdateGameGoal(-1);

            if (whereISpawned)
            {
                whereISpawned.UpdateEnemyNumber();
            }

            Destroy(gameObject);
        }

    }

    IEnumerator FlashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

    public void PlayWalkingAudio()
    {
        aud.PlayOneShot(audBossSteps[Random.Range(0, audBossSteps.Length)], audBossStepsVol);
    }

    void SetHealthBar()
    {
        // If the healthbar is not active, set it to active.
        if (!healthbar.gameObject.activeSelf)
        {
            healthbar.gameObject.SetActive(true);
        }

        healthbar.value = (float)HP / maxHP;
    }

}