using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    public WaveSpawner whereISpawned;

    [Header("----- Enemy Stats -----")]
    [SerializeField] int HP;
    [SerializeField] float shootRate;
    [SerializeField] int gold;

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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Get magnitude of normalized agent velocity.
        float animSpeed = agent.velocity.normalized.magnitude;

        //Set animator Speed float to lerp to velocity based off of animSpeedTrans.
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), animSpeed, animSpeedTrans * Time.deltaTime));

        PursuePlayer();
    }

    void PursuePlayer()
    {
        agent.SetDestination(gamemanager.instance.player.transform.position);

        // Get players direction.
        playerDir = gamemanager.instance.player.transform.position - headPos.position;
        Debug.DrawRay(headPos.position, playerDir);

        // Get raycast hit and from head position to player direction and store it in hit.
        RaycastHit hit;
        if(Physics.Raycast(headPos.position, playerDir, out hit, 15))
        {
            // If collider is the player start shooting.
            if (hit.collider.CompareTag("Player"))
            {
                if (!isShooting)
                {
                    StartCoroutine(Shoot());
                }
                
                // If remaining distance is less than or equal to stopping distance.
                if (agent.remainingDistance <= agent.stoppingDistance)
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
        anim.SetTrigger("Shoot");
        aud.PlayOneShot(audEnemyShoot[Random.Range(0, audEnemyShoot.Length)], audEnemyShootVol);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void CreateBullet()
    {
        Instantiate(Bullet, shootPos.position, transform.rotation);
    }

    public void TakeDamage(int amount)
    {
        HP -= amount;
        anim.SetTrigger("Damage");
        StartCoroutine(FlashRed());
        aud.PlayOneShot(audEnemyHurt[Random.Range(0, audEnemyHurt.Length)], audEnemyHurtVol);

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
        aud.PlayOneShot(audEnemySteps[Random.Range(0, audEnemySteps.Length)], audEnemyStepsVol);
    }
}
