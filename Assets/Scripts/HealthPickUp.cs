using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    [SerializeField] GameObject pickup;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            int currHP = GameObject.FindWithTag("Player").GetComponent<PlayerController>().HP;
            int maxHP = GameObject.FindWithTag("Player").GetComponent<PlayerController>().HPOrig;

            if (currHP < maxHP)
            {
                GameObject.FindWithTag("Player").GetComponent<PlayerController>().SetHP();
                GameObject.FindWithTag("Player").GetComponent<PlayerController>().CallHPUIUpdate();

                StartCoroutine(RespawnHP());
            }
        }
    }

    IEnumerator RespawnHP()
    {
        pickup.SetActive(false);

        yield return new WaitForSeconds(15);

        pickup.SetActive(true);
    }
}
