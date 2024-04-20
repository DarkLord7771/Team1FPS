using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpEffects powerUpEffect;
    [SerializeField] float powerUpTime;

    private void OnTriggerEnter(Collider other)
    {
        gamemanager.instance.playerScript.BeginPowerUp(powerUpEffect, powerUpTime);
        Destroy(gameObject);
    }
}
