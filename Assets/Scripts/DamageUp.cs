using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Power Up/DamageUp")]
public class DamageUp : PowerUpEffects
{
    [SerializeField] float damageModifier;

    void Update()
    {

    }

    public override IEnumerator ApplyEffect()
    {

        int startingDamage = gamemanager.instance.playerScript.GetCurrentDamage();
        yield return new WaitForSeconds(powerUpTime);
        gamemanager.instance.playerScript.shootDamage = gamemanager.instance.playerScript.GetCurrentDamage();
    }
}
