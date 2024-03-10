using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float spawnDelay;

    bool spawnedEnemy;

    // Start is called before the first frame update
    void Start()
    {
        
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

        Instantiate(enemy, spawnPoint.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(spawnDelay);
        spawnedEnemy = false;
    }
}
