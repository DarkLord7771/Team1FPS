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
        gamemanager.instance.playerScript.SetDamageMultiplier(damageModifier);
            
        yield return new WaitForSeconds(powerUpTime);
        gamemanager.instance.playerScript.damagePowerUp = false;
        gamemanager.instance.playerScript.SetDamageMultiplier(0);
        gamemanager.instance.playerScript.ResetDamage();
        
    }


}
