using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/DamageUp")]
public class DamageUp : PowerUpEffects
{
    [SerializeField] float damageModifier;

    public override IEnumerator ApplyEffect()
    {
        gamemanager.instance.playerScript.damagePowerUp = true;
        gamemanager.instance.playerScript.DamageMultiplier = damageModifier;
        gamemanager.instance.playerScript.gunHandler.IncreaseDamage();
            
        yield return new WaitForSeconds(powerUpTime);
        gamemanager.instance.playerScript.damagePowerUp = false;
        gamemanager.instance.playerScript.DamageMultiplier = 0;
        gamemanager.instance.playerScript.gunHandler.ResetDamage();
    }
}
