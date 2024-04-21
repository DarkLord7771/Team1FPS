using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealUp : MonoBehaviour
{
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip healClip;
    [Range(0, 1)] [SerializeField] float healClipVol;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gamemanager.instance.playerScript.HP != gamemanager.instance.playerScript.HPOrig)
        {
            gamemanager.instance.playerScript.PlayAudio(healClip, healClipVol);
            gamemanager.instance.playerScript.HP = gamemanager.instance.playerScript.HPOrig;
            gamemanager.instance.playerScript.UpdatePlayerUI();

            if (gamemanager.instance.playerScript.lowHealth)
            {
                gamemanager.instance.playerScript.lowHealth = false;
            }

            Destroy(gameObject);
        }
    }
}
