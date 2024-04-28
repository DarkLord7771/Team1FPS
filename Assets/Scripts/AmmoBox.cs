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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gamemanager.instance.playerScript.gunHandler.HasMissingAmmo())
        {
            aud.PlayOneShot(audOpen, audOpenVol);
            lid.transform.Rotate(lidOpenRot);
            gamemanager.instance.playerScript.gunHandler.RefillAmmo();
            
            hasRefilledAmmo = true;
        }
        else if (gamemanager.instance.playerScript.gunHandler.HasGuns() && !gamemanager.instance.playerScript.gunHandler.HasMissingAmmo())
        {
            hasRefilledAmmo = false;
            StartCoroutine(gamemanager.instance.DisplayMessage(gamemanager.instance.menuFullAmmo));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (hasRefilledAmmo)
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
        else
        {
            gamemanager.instance.menuFullAmmo.SetActive(false);
        }
    }
}
