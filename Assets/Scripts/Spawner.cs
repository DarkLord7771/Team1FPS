using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject enemy1;
    [SerializeField] GameObject enemy2;
    [SerializeField] GameObject enemy3;
    [SerializeField] GameObject spawnPoint;
    [SerializeField] float spawnDelay;

    bool spawnedEnemy;
    bool spawnerEmpty;

    // Start is called before the first frame update
    void Start()
    {
        spawnerEmpty = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Spawn Enemy 1 if "Fire2" is pressed.
        if (Input.GetButtonDown("Fire2") && !spawnedEnemy)
        {
            StartCoroutine(SpawnEnemy1());
        }
        // Spawn Enemy 2 if 2 is pressed.
        else if (Input.GetKeyDown(KeyCode.Alpha2) && !spawnedEnemy)
        {
            StartCoroutine(SpawnEnemy2());
        }
        // Spawn Enemy 3 if 3 is pressed.
        else if (Input.GetKeyDown(KeyCode.Alpha3) && !spawnedEnemy)
        {
            StartCoroutine(SpawnEnemy3());
        }
    }

   IEnumerator SpawnEnemy1()
    {
        spawnedEnemy = true;

        if (spawnerEmpty)
        {
            Instantiate(enemy1, spawnPoint.transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(spawnDelay);
        spawnedEnemy = false;
    }

    IEnumerator SpawnEnemy2()
    {
        spawnedEnemy = true;

        if (spawnerEmpty)
        {
            Instantiate(enemy2, spawnPoint.transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(spawnDelay);
        spawnedEnemy = false;
    }

    IEnumerator SpawnEnemy3()
    {
        spawnedEnemy = true;

        if (spawnerEmpty)
        {
            Instantiate(enemy3, spawnPoint.transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(spawnDelay);
        spawnedEnemy = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        spawnerEmpty = false;
    }

    private void OnTriggerExit(Collider other)
    {
        spawnerEmpty = true;
    }
}
