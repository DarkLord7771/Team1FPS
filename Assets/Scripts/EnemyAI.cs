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
    [SerializeField] GameObject Bullet;

    [Header("-----Enemy Stats-----")]
    [SerializeField] int HP;
    [SerializeField] float shootRate;

    bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        gamemanager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(gamemanager.instance.player.transform.position);

        if (!isShooting)
        {
            StartCoroutine(shoot());
        }

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
