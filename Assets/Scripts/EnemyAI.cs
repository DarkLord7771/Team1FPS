using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [Header("-----Components-----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] GameObject Bullet;
    Animator enemyAnimator;

    [Header("-----Enemy Stats-----")]
    [SerializeField] int HP;
    [SerializeField] float shootRate;
    [SerializeField] int faceTargetSpeed;

    bool isShooting;
    Vector3 playerDir;

    // Start is called before the first frame update
    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        gamemanager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        AttackPlayer();
    }

    void AttackPlayer()
    {
        MoveTowardPlayer();

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
                    StartCoroutine(shoot());
                }
                
                // If remaining distance is less than or equal to stopping distance.
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    // Stop animation and rotate toward player.
                    enemyAnimator.SetBool("isMoving", false);
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

    void MoveTowardPlayer()
    {
        // Move enemy toward player and set animation to running animation.
        agent.SetDestination(gamemanager.instance.player.transform.position);
        enemyAnimator.SetBool("isMoving", true);
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(Bullet, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void TakeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashRed());

        if (HP <= 0)
        {
            gamemanager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }

    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }
}
