using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public static WaveManager instance;
    public WaveSpawner[] spawners;
    public int timeBetweenSpawns;

    public int waveCurrent;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        StartCoroutine(StartWave());

        // Check if gamemanager.instance is not null or if started first.
        if (gamemanager.instance)
        {
            //Debug.Log("GM First");
            gamemanager.instance.SetWaveCount();
        }
    }

    public IEnumerator StartWave()
    {
        waveCurrent++;

        if (waveCurrent <= spawners.Length)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);
            spawners[waveCurrent - 1].StartWave();
        }
    }
}
