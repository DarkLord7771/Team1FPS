using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    [SerializeField] GameObject lid;
    [SerializeField] Vector3 lidOpenRot;
    [SerializeField] float destroyTime;
    Quaternion lidRot;

    public AmmoSpawner whereISpawned;
    public int spawnIndex;
    bool hasRefilledAmmo;

    // Start is called before the first frame update
    void Start()
    {
        lidRot = lid.transform.rotation;

        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log(other.name);

        if (other.CompareTag("Player") && gamemanager.instance.playerScript.HasMissingAmmo())
        {
            lid.transform.Rotate(lidOpenRot);
            gamemanager.instance.playerScript.RefillAmmo();
            
            hasRefilledAmmo = true;
        }
        else if (gamemanager.instance.playerScript.HasGuns() && !gamemanager.instance.playerScript.HasMissingAmmo())
        {
            hasRefilledAmmo = false;
            gamemanager.instance.menuFullAmmo.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (hasRefilledAmmo)
        {
            lid.transform.rotation = lidRot;

            if (whereISpawned)
            {
                whereISpawned.StartSpawn(spawnIndex);
            }

            Destroy(gameObject, destroyTime);
            hasRefilledAmmo = false;
        }
        else
        {
            gamemanager.instance.menuFullAmmo.SetActive(false);
        }
    }
}
