using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] PowerUpEffects powerUpEffect;

    private void OnTriggerEnter(Collider other)
    {
        gamemanager.instance.CreatePowerUpDisplay(powerUpEffect);

        gamemanager.instance.playerScript.BeginPowerUp(powerUpEffect);
        Destroy(gameObject);
    }
}
