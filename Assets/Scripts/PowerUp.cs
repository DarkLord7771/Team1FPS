using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip audPowerUp;
    [Range(0, 1)][SerializeField] float audPowerUpVol;

    [SerializeField] PowerUpEffects powerUpEffect;

    private void OnTriggerEnter(Collider other)
    {
        powerUpEffect.remainingTime = powerUpEffect.powerUpTime;

        gamemanager.instance.PowerUpDisplay.CreateDisplay(powerUpEffect);

        gamemanager.instance.playerScript.BeginPowerUp(powerUpEffect);
        Destroy(gameObject);
    }
}
