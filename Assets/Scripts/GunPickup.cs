using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    [SerializeField] GunStats gun;

    // Start is called before the first frame update
    void Start()
    {
        gun.ammoCur = gun.ammoMax;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gamemanager.instance.playerScript.GetGunStats(gun);
            Destroy(gameObject);
        }
    }

}
