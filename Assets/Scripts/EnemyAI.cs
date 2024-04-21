using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] GameObject Bullet;
    [SerializeField] AudioSource aud;
    [SerializeField] Slider healthbar;
    [SerializeField] GameObject shield;
    public WaveSpawner whereISpawned;

    [Header("----- Enemy Stats -----")]
    [SerializeField] int HP;
    int maxHP;
    [SerializeField] float shootRate;
    [SerializeField] int gold;
    [SerializeField] int dropRate;
    [SerializeField] float offset;
    [SerializeField] int raycastDistance;
    [SerializeField] bool isShielded;

    [Header("----- Enemy Locomotion -----")]
    [SerializeField] int faceTargetSpeed;
    [SerializeField] float animSpeedTrans;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audEnemyHurt;
    [Range(0, 1)][SerializeField] float audEnemyHurtVol;
    [SerializeField] AudioClip[] audEnemySteps;
    [Range(0, 1)][SerializeField] float audEnemyStepsVol;
    [SerializeField] AudioClip[] audEnemyShoot;
    [Range(0, 1)][SerializeField] float audEnemyShootVol;

    bool isShooting;
    Vector3 playerDir;
    WeaponIk weaponIk;
    Kamikaze kamikaze;
    Heavy heavy;

    // Start is called before the first frame update
    void Start()
    {
        maxHP = HP;

        // Find healthbar and set it to not active on start.
        healthbar = GameObject.Find("Health Bar").GetComponent<Slider>();
        healthbar.gameObject.SetActive(false);

        weaponIk = GetComponent<WeaponIk>();
        weaponIk.SetAimTransform(shootPos);

        TryGetComponent<Kamikaze>(out kamikaze);
        if (kamikaze != null)
        {
            raycastDistance = (int)agent.stoppingDistance;
        }

        TryGetComponent<Heavy>(out heavy);

        if (isShielded)
        {
            Instantiate(shield, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Get magnitude of normalized agent velocity.
        float animSpeed = agent.velocity.normalized.magnitude;

        //Set animator Speed float to lerp to velocity based off of animSpeedTrans.
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), animSpeed, animSpeedTrans * Time.deltaTime));

        if (healthbar != null)
        {
            healthbar.transform.rotation = Camera.main.transform.rotation;
        }
        
        PursuePlayer();

        if (!isShooting && kamikaze == null)
        {
            StartCoroutine(Shoot());
        }
    }

    void PursuePlayer()
    {
        agent.SetDestination(gamemanager.instance.player.transform.position);

        // Get players direction.
        playerDir = gamemanager.instance.player.transform.position - headPos.position;
        weaponIk.SetTargetTransform(gamemanager.instance.player.transform);

        // Get raycast hit and from head position to player direction and store it in hit.
        RaycastHit hit;
        if(Physics.Raycast(headPos.position, playerDir, out hit, raycastDistance))
        {
            // If collider is the player start shooting.
            if (hit.collider.CompareTag("Player"))
            {
                // If remaining distance is less than or equal to stopping distance.
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    FaceTarget();
                }

                if (kamikaze != null)
                {
                    kamikaze.Explode();
                    gamemanager.instance.playerScript.PlayAudio(kamikaze.explosionClip, kamikaze.volume);
                    hit.collider.GetComponent<IDamage>().TakeDamage(kamikaze.explosionDamage);
                    TakeDamage(HP);
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
        anim.SetTrigger("Shoot");
        aud.PlayOneShot(audEnemyShoot[Random.Range(0, audEnemyShoot.Length)], audEnemyShootVol);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void CreateBullet()
    {
        Instantiate(Bullet, shootPos.position, shootPos.rotation);
    }

    public void TakeDamage(int amount)
    {
        if (heavy != null && !heavy.IsLaserShot())
        {
            amount = (int)Mathf.Ceil(amount * 0.5f);
        }
        else if (isShielded && !gamemanager.instance.playerScript.IsNotLaserWeapon())
        {
            amount *= 2;
        }
        else if (isShielded && !gamemanager.instance.playerScript.IsNotLaserWeapon())
        {
            amount /= 2;
        }

        HP -= amount;
        anim.SetTrigger("Damage");
        StartCoroutine(FlashRed());
        aud.PlayOneShot(audEnemyHurt[Random.Range(0, audEnemyHurt.Length)], audEnemyHurtVol);
        SetHealthBar();

        if (HP <= 0)
        {
            gamemanager.instance.playerScript.SetGold(gold);
            gamemanager.instance.UpdateGoldDisplay();
            gamemanager.instance.UpdateGameGoal(-1);

            if (whereISpawned)
            {
                whereISpawned.UpdateEnemyNumber();
            }

            if (Random.Range(0, dropRate) % dropRate == 0)
            {
                Vector3 spawnPosition = transform.position;
                spawnPosition.y = offset;

                Instantiate(gamemanager.instance.powerUps[Random.Range(0, gamemanager.instance.powerUps.Length - 1)], spawnPosition, transform.rotation);
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
        aud.PlayOneShot(audEnemySteps[Random.Range(0, audEnemySteps.Length)], audEnemyStepsVol);
    }

    void SetHealthBar()
    {
        // If the healthbar is not active, set it to active.
        if (!healthbar.gameObject.activeSelf)
        {
            healthbar.gameObject.SetActive(true);
        }

        if (healthbar != null)
        {
            healthbar.value = (float)HP / maxHP;
        }
    }
}
