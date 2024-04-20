using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] GameObject lid;
    [SerializeField] Vector3 lidOpenRot;
    [SerializeField] float destroyTime;
    Quaternion lidRot;
    GameObject fullAmmoMenu;

    [Header("----- Audio -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip audOpen;
    [Range(0, 1)][SerializeField] float audOpenVol;
    [SerializeField] AudioClip audClose;
    [Range(0, 1)][SerializeField] float audCloseVol;

    public AmmoSpawner whereISpawned;
    public int spawnIndex;
    bool hasRefilledAmmo;

    // Start is called before the first frame update
    void Start()
    {
        lidRot = lid.transform.rotation;
        fullAmmoMenu = gamemanager.instance.menuFullAmmo;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log(other.name);

        if (other.CompareTag("Player") && gamemanager.instance.playerScript.HasMissingAmmo())
        {
            aud.PlayOneShot(audOpen, audOpenVol);
            lid.transform.Rotate(lidOpenRot);
            gamemanager.instance.playerScript.RefillAmmo();
            
            hasRefilledAmmo = true;
        }
        else if (other.CompareTag("Player") && gamemanager.instance.playerScript.HasGuns() && !gamemanager.instance.playerScript.HasMissingAmmo())
        {
            hasRefilledAmmo = false;
            gamemanager.instance.SetDisplayMessageActive(fullAmmoMenu);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && hasRefilledAmmo)
        {
            aud.PlayOneShot(audClose, audCloseVol);
            lid.transform.rotation = lidRot;

            if (whereISpawned)
            {
                whereISpawned.StartSpawn(spawnIndex);
            }


            Destroy(gameObject, destroyTime);
            hasRefilledAmmo = false;
        }
        else if (other.CompareTag("Player"))
        {
            gamemanager.instance.SetDisplayMessageActive(fullAmmoMenu);
        }
    }
}
