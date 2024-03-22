using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] GameObject objectToSpwan;
    [SerializeField] int numToSpawn;
    [SerializeField] int spawnTimer;
    [SerializeField] Transform[] spawnPos;

    int spawnCount;
    bool isSpawning;
    bool startSpawning;
    int numberKilled;

    // Start is called before the first frame update
    void Start()
    {
        gamemanager.instance.updateGameGoal(numToSpawn);

    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawning && !isSpawning && spawnCount < numToSpawn)
        {
            StartCoroutine(spawn());

        }
    }

    public void startWave()
    {
        startSpawning = true;
        gamemanager.instance.updateGameGoal(numToSpawn);
    }

    public IEnumerator spawn()
    {
        isSpawning = true;
        int arrayPos = Random.Range(0, spawnPos.Length);
        GameObject objectSpawned = Instantiate(objectToSpwan, spawnPos[arrayPos].transform.position, spawnPos[arrayPos].transform.rotation);

        if(objectSpawned.GetComponent<EnemyAI>())
            objectSpawned.GetComponent<EnemyAI>().whereISpawned = this;
        spawnCount++;
        yield return new WaitForSeconds(spawnTimer);
        isSpawning = false;


    }

    public void updateEnemyNumber()
    {
        {
            numberKilled++;

            if(numberKilled >= numToSpawn)
            {
                startSpawning = false;
                StartCoroutine(WaveManager.instance.startWave());

            }
        }
    }  

}