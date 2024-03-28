using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSpawner : MonoBehaviour
{
    [SerializeField] Transform[] ammoSpawnPos;
    [SerializeField] GameObject ammoBox;
    [SerializeField] float spawnTimer;

    int spawnIndex;
    bool isSpawning;
    bool startSpawning;
    bool spawnerEmpty;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < ammoSpawnPos.Length; i++)
        {
            SpawnAmmoBox(i);
        }
    }

    private void Update()
    {
        if (startSpawning && !isSpawning)
        {
            StartCoroutine(Respawn(spawnIndex));
        }
    }

    void SpawnAmmoBox(int index)
    {
            GameObject objectSpawned = Instantiate(ammoBox, ammoSpawnPos[index].transform.position, ammoSpawnPos[index].transform.rotation);

            if (objectSpawned.GetComponent<AmmoBox>())
            {
                objectSpawned.GetComponent<AmmoBox>().whereISpawned = this;
                objectSpawned.GetComponent<AmmoBox>().spawnIndex = index;
            }
    }

    IEnumerator Respawn(int index)
    {
        isSpawning = true;
        yield return new WaitForSeconds(spawnTimer);
        SpawnAmmoBox(index);

        isSpawning = false;
    }

    public void StartSpawn(int index)
    {
        startSpawning = true;
        spawnIndex = index;
    }
}
