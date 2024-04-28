using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManagerV2 : MonoBehaviour
{

    public static WaveManagerV2 instance;

    [SerializeField] GameObject[] enemyTypes;
    [SerializeField] GameObject[] spawnPos;

    [SerializeField] int waveCount;
    [SerializeField] int spawnTimer;

    int currWave;
    bool waveActive;
    int diffMod;
    bool isSpawning;
    int spawnedEnemyCount;
    

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (waveActive)
        {
            int enemies = SetEnemyCount(currWave);

            if (spawnedEnemyCount <= enemies)
                StartCoroutine(SpawnEnemy());
        }
    }

    int SetEnemyCount(int waveNum) //Sets ever increasing enemy count, but levels slightly after enough rounds, sqrt function "plateaus"
    {
        return Mathf.FloorToInt(Mathf.Sqrt(waveNum + 10) + (Mathf.Sqrt(waveNum) * diffMod));
    }

    int ChooseEnemyListBound() //Chooses what type of enemy to spawn based off of current wave
    {
        int temp = 0;

        if (currWave < 3)
            temp = 0;
        else if (currWave >= 3 && currWave < 7)
            temp = 1;
        else if (currWave >=7 && currWave < 10)
            temp = 2;
        else if (currWave >= 10)
            temp = enemyTypes.Length - 1;

        return temp;
    }

    IEnumerator SpawnEnemy() //spawns enemy based off of round and random spawner
    {
        isSpawning = true;
        int spawnArrayPos = Random.Range(0, spawnPos.Length);
        int enemyArrayPos = Random.Range(0, ChooseEnemyListBound());
        Instantiate(enemyTypes[enemyArrayPos], spawnPos[spawnArrayPos].transform.position, spawnPos[spawnArrayPos].transform.rotation);
        spawnedEnemyCount++;
        yield return new WaitForSeconds(spawnTimer);
        isSpawning = false;
    }


}
