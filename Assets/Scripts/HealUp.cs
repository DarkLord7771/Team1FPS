using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gamemanager.instance.playerScript.HP = gamemanager.instance.playerScript.HPOrig;
            gamemanager.instance.playerScript.UpdatePlayerUI();

            Destroy(gameObject);
        }
    }
}
