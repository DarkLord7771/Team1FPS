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
    [SerializeField] int enemyCount;

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
        // If R-Mouse Button is pressed and spawnedEnemy is false, start SpawnWave.
        if (Input.GetButtonDown("Fire2") && !spawnedEnemy)
        {
            StartCoroutine(SpawnWave(enemyCount));
        }
    }

    IEnumerator SpawnWave(int enemies)
    {
        // Spawn specified number of enemies.
        for (int i = 0; i <= enemies; i++)
        {
            StartCoroutine(SpawnEnemy());
            yield return new WaitForSeconds(spawnDelay);
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

    IEnumerator SpawnEnemy()
    {
        spawnedEnemy = true;

        // Get random number to spawn random enemy.
        int enemy = Random.Range(0, 3);

        // Spawn enemy based on random int.
        switch (enemy)
        {
            case 0:
                Instantiate(enemy1, spawnPoint.transform.position, Quaternion.identity);
                break;
            case 1:
                Instantiate(enemy2, spawnPoint.transform.position, Quaternion.identity);
                break;
            case 2:
                Instantiate(enemy3, spawnPoint.transform.position, Quaternion.identity);
                break;
        }

        // Wait for spawn delay and set spawnedEnemy to false.
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
