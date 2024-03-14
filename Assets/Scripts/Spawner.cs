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
        if (Input.GetButtonDown("Fire2") && !spawnedEnemy)
        {
            StartCoroutine(SpawnEnemy());
        }
    }

   IEnumerator SpawnEnemy()
    {
        spawnedEnemy = true;

        if (spawnerEmpty)
        {
            Instantiate(enemy1, spawnPoint.transform.position, Quaternion.identity);
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
