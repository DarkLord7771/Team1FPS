using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public static WaveManager instance;
    [SerializeField] WaveSpawner[] spawners;
    [SerializeField] int timeBetweenSpawns;

    public int waveCurrent;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        StartCoroutine(startWave());

    }

    public IEnumerator startWave()
    {
        foreach (WaveSpawner spawner in spawners)
        {
            waveCurrent++;

            if(waveCurrent <= spawners.Length)
            {
                yield return new WaitForSeconds(timeBetweenSpawns);
                spawners[waveCurrent -1].startWave();
            }
        }
    }
}
